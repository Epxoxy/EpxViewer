using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using EpxViewer.ListBox;
using System.Windows.Input;
using System.Diagnostics;

namespace EpxViewer
{
    /// <summary>
    /// Interaction logic for MenuPan.xaml
    /// </summary>
    public partial class MenuPan : UserControl, IShowInfo
    {
        public Command HideCommand { get; private set; }
        public int MainSelectedIndex
        {
            get { return FrdNavigate.SelectedIndex; }
            set { FrdNavigate.SelectedIndex = value; }
        }
        private NavItem sysNavItem;
        private const string defaultTitle = "Unknow";

        #region Contructor

        public MenuPan()
        {
            InitializeComponent();
        }

        #endregion

        #region Override
        
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            HideCommand = new Command();
            HideCommand.ExecuteCommand = o =>
            {
                updateUnloaded();
                var popup = this.Parent as System.Windows.Controls.Primitives.Popup;
                if (popup != null) ExtPopup.SetIsInView(popup, false);
            };

            selectionForwardHistory = new List<int>();
            selectionBackHistory = new List<int>();

            //Initilize MainMenus
            if (!loadLogMenu()) initDefaultMenus();
            NavMenus = new BindingList<BindingList<NavItem>>();

            //Binding and initilized state
            FrdNavigate.ItemsSource = MainNavs;
            SubNavigate.ItemsSource = NavMenus;
            MsgPane.Visibility = Visibility.Collapsed;
            Indicator.Visibility = Visibility.Collapsed;
            createAnimation();
            Debug.WriteLine("Initilized");
        }

        #endregion

        #region Menus operate

        private void initSystemMenu()
        {
            var textblock = new TextBlock();
            textblock.IsVisibleChanged += async (o, arg) =>
            {
                while (textblock.IsVisible)
                {
                    textblock.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    await System.Threading.Tasks.Task.Delay(1000);
                }
            };

            BindingList<NavItem> settingMenu;
            NavItem.getListNavItem(out settingMenu, "Help(?)", "Debug", "Exit", "Exit(sys)", "Unknow");
            foreach(var item in settingMenu)
            {
                item.CanEdit = false;
            }
            settingMenu[1].RunPath = Environment.CurrentDirectory;
            settingMenu[2].Command = HideCommand;
            settingMenu[3].Command = SpecialCommand.ExitAppCommand;

            sysNavItem = new NavItem("S") {  Title = "Setting", SubMenus = settingMenu, Content = textblock,CanEdit = false};
        }

        private void initDefaultMenus()
        {
            BindingList<NavItem> menus = new BindingList<NavItem>();

            var systems = new NavItem("System0-0", "System1-0", "Disk1-0", "Memory1-0", "Require1-0", "Other1-0");
            var disk = new NavItem("Disk0-1", "System1-1", "Disk1-1", "Memory1-1", "Require1-1", "Other1-1");
            var memory = new NavItem("Memory0-2", "System1-2", "Disk1-2", "Memory1-2", "Require1-2", "Other1-2");
            var require = new NavItem("Require0-3", "System1-3", "Disk1-3", "Memory1-3", "Require1-3", "Other1-3");
            var other = new NavItem("Other0-4");

            BindingList<NavItem> requeireSub, systemSub;
            NavItem.getListNavItem(out requeireSub, "System2-3", "Disk2-3", "Memory2-3", "Require2-3", "Other2-3");
            NavItem.getListNavItem(out systemSub, "System2-0", "Disk2-0", "Memory2-0", "Require2-0", "Other2-0");

            require.SubMenus[1].SubMenus = requeireSub;
            systems.SubMenus[2].SubMenus = systemSub;

            menus.Add(systems);
            menus.Add(disk);
            menus.Add(memory);
            menus.Add(require);
            menus.Add(other);

            //NavMenus.Add(menus);
            MainNavs = new BindingList<NavItem>();
            MainNavs.Add(new NavItem("A") { Title = "Basic", SubMenus = menus, Content = "A part, Hello, Welcome", Description = "Main menu" });
            MainNavs.Add(new NavItem("B") { Title = "Empty",  Content = "B part, second one, noting special, state : null", Description = "Something description" });
            MainNavs.Add(new NavItem("C") { Title = "C part", Content = "C part" });
            initSystemMenu();
            MainNavs.Add(sysNavItem);
        }

        public void loadCustomMenus(BindingList<NavItem> navItemList)
        {
            var navItems = new BindingList<NavItem>();
            foreach(var navitem in navItemList)
            {
                navItems.Add(navitem);
            }
            MainNavs = navItems;
        }

        public void saveMenu()
        {
            BindingList<NavItem> saveList;
            saveList = MainNavs;
            saveList.Remove(sysNavItem);
            Logger.getLogger().logValue("MainNavs", saveList);
        }

        private bool loadLogMenu()
        {
            var obj = Logger.getLogger().getLogOfObject("MainNavs");
            if(obj != null)
            {
                var list = obj as BindingList<NavItem>;
                if (list != null) {
                    MainNavs = list;
                    if (sysNavItem == null) initSystemMenu();
                    MainNavs.Add(sysNavItem);
                    return true;
                }
            }
            return false;
        }

        public BindingList<BindingList<NavItem>> NavMenus { get; private set; }
        public BindingList<NavItem> MainNavs { get; private set; }
        #endregion

        #region Animation

        private void createAnimation()
        {
            viewIndicatorAnimation = new ObjectAnimationUsingKeyFrames();
            Storyboard.SetTarget(viewIndicatorAnimation, Indicator);
            Storyboard.SetTargetProperty(viewIndicatorAnimation, new PropertyPath(UIElement.VisibilityProperty));
            viewIndicatorAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Collapsed, TimeSpan.FromSeconds(0d)));
            viewIndicatorAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Visible, TimeSpan.FromSeconds(0.32d)));

            renderAnimation = new DoubleAnimation();
            Storyboard.SetTarget(renderAnimation, container);
            Storyboard.SetTargetProperty(renderAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            renderAnimation.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut };
            renderAnimation.Duration = TimeSpan.FromSeconds(0.3d);
            renderAnimation.To = 0;

            storyboard = new Storyboard();
            storyboard.Children.Add(renderAnimation);
            storyboard.Children.Add(viewIndicatorAnimation);

            Timeline.SetDesiredFrameRate(storyboard, 60);
            RenderOptions.SetBitmapScalingMode(storyboard, BitmapScalingMode.LowQuality);
        }

        private void udpateWidthAnimation()
        {
            int count = NavMenus.Count;
            if (count == 0) count = 1;

            double newTranslate = (-count * 178) + 520;
            renderAnimation.To = newTranslate > 0 ? newTranslate : 0;
            if (newTranslate == preTranslate) return;

            preTranslate = newTranslate;
            storyboard.Begin();
            System.Diagnostics.Debug.WriteLine("translate  " + newTranslate);
        }

        private void updateIndicatorState(Visibility newState)
        {
            if (newState == Visibility.Collapsed) Indicator.Width = 0;
            else Indicator.Width = 28d;
        }

        private ObjectAnimationUsingKeyFrames viewIndicatorAnimation;
        private DoubleAnimation renderAnimation;
        private Storyboard storyboard;
        private double preTranslate = 0;
        #endregion

        #region IShowInfo

        public void ShowMsgPane(NavItem navItem)
        {
            if(navItem != null && navItem.Content != null)
            {
                ShowMsgPane(string.IsNullOrEmpty(navItem.Title) ? defaultTitle : navItem.Title, navItem.Content, navItem.Description);
            }
        }
        
        public void ShowMsgPane(string title, object content, object description)
        {
            MsgPane.Title = title;
            MsgPane.AlertContent = content;
            MsgPane.Description = description;
            if (MsgPane.Visibility != Visibility.Visible)
                MsgPane.Visibility = Visibility.Visible;
        }

        public void CollapsedMsgPane()
        {
            if (MsgPane.Visibility != Visibility.Collapsed)
                MsgPane.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Event

        #region Mouse event

        private void itemMouseRightDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("itemMouseRightDown");
            if (e.ClickCount == 2) { updateNavItem(sender); e.Handled = true; }
            //e.Handled = true;
        }

        private void Bd_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnLeftButton down " + e.ClickCount);
        }

        #endregion

        #region Selection event

        private void onFrdNavigateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check and update menu
            if (e.AddedItems.Count < 1) return;
            else updateMainMenus(e.AddedItems[0] as NavItem);
            scrollToTop(MainSelectedIndex);
            //Mark handled
            e.Handled = true;
        }

        private void onSubNavigateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1) return;
            else updateSubMenus(e.AddedItems[0] as NavItem);
            e.Handled = true;
        }
        
        private void scrollToTop(int index)
        {
            List<NavItem> rmList = new List<NavItem>();
            for(int i = 0; i < index; ++i)
            {
                rmList.Add(MainNavs[i]);
            }
            foreach(var rm in rmList)
            {
                MainNavs.Remove(rm);
                MainNavs.Add(rm);
            }
        }

        #endregion

        private void containerPreviewMouseLBtnDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Right)
            {
                if (e.RightButton == MouseButtonState.Pressed) e.Handled = true;
                else e.Handled = false;
            }
            else
            {
                if (e.ChangedButton == MouseButton.XButton2) Debug.WriteLine("Forward");
                else if (e.ChangedButton == MouseButton.XButton1) Debug.WriteLine("Back");
                e.Handled = true;
            }
            Debug.WriteLine(e.RightButton == MouseButtonState.Released);
            Debug.WriteLine(e.ChangedButton.ToString());
        }

        private void editMouseRBtnDownUp(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Released)
            {
                updateNavItem(sender);
                e.Handled = true;
                Debug.WriteLine("editPreviewMouseDownUp");
            }else
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Update

        private void updateMainMenus(NavItem navItem)
        {
            NavMenus.Clear();
            udpateWidthAnimation();
            if (navItem == null || navItem.Content == null)
            {
                CollapsedMsgPane();
                updateIndicatorState(Visibility.Collapsed);
            }
            if (navItem != null)
            {
                //Show menu item description
                if(navItem.Content != null) ShowMsgPane(navItem);
                //Show subMenus and update indication state
                if (navItem.SubMenus != null)
                {
                    NavMenus.Add(navItem.SubMenus);
                    updateIndicatorState(Visibility.Visible);
                }
                else
                {
                    updateIndicatorState(Visibility.Collapsed);
                }
            }
        }

        private void updateSubMenus(NavItem navitem)
        {
            if (navitem == null) return;
            if (navitem.Command != null) navitem.Command.Execute(null);
            if (navitem.Content != null) ShowMsgPane(navitem);

            System.Diagnostics.Debug.WriteLine("Count " + NavMenus.Count + ", Index " + navitem.Index);
            //Clear old menus
            int rmIndex = 0, count = NavMenus.Count;
            rmIndex = navitem.SubMenus == null ? (navitem.Index) : (navitem.SubMenus[0].Index - 1);
            for (int i = rmIndex; i < count; ++i)
            {
                NavMenus.RemoveAt(rmIndex);
            }
            //Add new submenus
            int delayTime = 0;
            if (navitem.SubMenus != null)
            {
                NavMenus.Add(navitem.SubMenus);
                delayTime = 300;
            }
            if (count != NavMenus.Count) udpateWidthAnimation();
            if (navitem.RunPath != null) runProcess(navitem.RunPath, delayTime);
        }

        private async void runProcess(string path, int delayTime = 0)
        {
            if (delayTime > 0) await System.Threading.Tasks.Task.Delay(delayTime);
            using (CreateProcess app = new CreateProcess())
            {
                app.startWindow(path);
            }
        }
        
        private void updateNavItem(object sender)
        {
            var item = (sender as FrameworkElement).Tag;
            if (item != null)
            {
                var navitem = item as NavItem;
                if (navitem != null && navitem.CanEdit)
                {
                    EditMenuPane editMenuPane = new EditMenuPane();
                    editMenuPane.NavName = navitem.Menu;
                    editMenuPane.RunPath = navitem.RunPath;
                    if (Alert.Show("Edit", editMenuPane) == AlertResult.OK)
                    {
                        NavItem newItem = navitem;
                        string newName = editMenuPane.NavName;
                        string newPath = editMenuPane.RunPath;
                        editMenuPane = null;
                        if (newName == navitem.Menu && newPath == navitem.RunPath) return;
                        if (newName != navitem.Menu) { newItem.Menu = newName; }
                        if (newPath != navitem.RunPath) { newItem.RunPath = newPath; };

                        int index;
                        if (navitem.Index > 0)
                        {
                            index = NavMenus[navitem.Index - 1].IndexOf(navitem);
                            if (index > -1) NavMenus[navitem.Index - 1][index] = newItem;
                        }
                        else
                        {
                            index = MainNavs.IndexOf(navitem);
                            if (index > -1) MainNavs[index] = newItem;
                        }
                    }
                }
                else
                {
                    updateWaring("Can't edit object!");
                }
            }
        }

        private bool isWaring;
        private int checkTime = 3000;
        private async void updateWaring(string waringMsg)
        {
            if (isWaring)
            {
                ShowMsgPane("Waring", waringMsg, DateTime.Now.ToString());
                checkTime = 3000;
            }
            else
            {
                string title = MsgPane.Title;
                object content = MsgPane.AlertContent;
                object description = MsgPane.Description;

                isWaring = true;
                checkTime = 3000;
                ShowMsgPane("Waring", waringMsg, DateTime.Now.ToString());

                while (checkTime != 0)
                {
                    await System.Threading.Tasks.Task.Delay(1000);
                    checkTime -= 1000;
                }
                ShowMsgPane(title, content, description);
                isWaring = false;
            }
        }

        //Recycling
        private void initilizeTextBox(TextBox textbox)
        {
            textbox = new TextBox();
            textbox.Width = 300;
            textbox.TextWrapping = TextWrapping.Wrap;
            textbox.BorderThickness = new Thickness(0);
            textbox.Background = Brushes.Transparent;
            textbox.MaxLines = 1;
        }

        public void updateUnloaded()
        {
            FrdNavigate.SelectedIndex = -1;
            updateMainMenus(null);
        }
        
        #endregion

        #region Forward or back

        private void forwardSelected()
        {
        }
        private void backSelected()
        {
        }

        private List<int> selectionForwardHistory;
        private List<int> selectionBackHistory;
        private const int maxHistoryCount = 10;
        #endregion
    }

    [Serializable]
    public class NavItem
    {
        #region Members

        private int index = 0;
        private string menu;
        private string title;
        private BindingList<NavItem> subMenus;
        private object content;
        private string description;
        private string runPath;
        private bool canEdit = true;

        #endregion

        #region Properities

        public int Index
        {
            get { return index; }
            private set
            {
                index = value;
                updateIndex();
            }
        }
        public string Menu
        {
            get { return menu; }
            set { menu = value; }
        }
        public BindingList<NavItem> SubMenus
        {
            get { return subMenus; }
            set
            {
                subMenus = value;
                updateIndex();
            }
        }
        public object Content
        {
            get { return content; }
            set { content = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string RunPath
        {
            get { return runPath; }
            set { runPath = value; }
        }
        public ICommand Command { get; set; }
        public bool CanEdit
        {
            get { return canEdit; }
            set { canEdit = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        #endregion

        #region Static

        public static void getListNavItem(out BindingList<NavItem> navItems, params string[] itemNames)
        {
            navItems = new BindingList<NavItem>();
            foreach (var itemName in itemNames)
            {
                navItems.Add(new NavItem(itemName));
            }
        }

        #endregion

        #region Private Method

        private void updateIndex()
        {
            if (subMenus == null) return;
            int newIndex = index + 1;
            foreach (var subMenu in subMenus)
            {
                if(subMenu.Index != newIndex)
                    subMenu.Index = newIndex;
            }
        }

        #endregion

        #region Public Method

        public void addSubMenus(params string[] subNames)
        {
            if (subNames.Length < 1) return;

            var menusList = new BindingList<NavItem>();
            foreach (var itemName in subNames)
            {
                menusList.Add(new NavItem(itemName));
            }
            SubMenus = menusList;
        }
        
        #endregion

        #region Contructor

        public NavItem() { }
        public NavItem(string menuName)
        {
            Menu = menuName;
        }
        public NavItem(string menuName, params string[] subNames)
        {
            Menu = menuName;
            addSubMenus(subNames);
        }

        #endregion

        #region Override

        public override string ToString()
        {
            if(string.IsNullOrEmpty(Menu))
                return base.ToString();
            return Menu;
        }

        #endregion
    }
}
