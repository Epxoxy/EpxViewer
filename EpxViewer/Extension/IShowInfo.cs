using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpxViewer
{
    public interface IShowInfo
    {
        void ShowMsgPane(NavItem navItem);
        void ShowMsgPane(string title, object content, object description);
        void CollapsedMsgPane();
    }
}
