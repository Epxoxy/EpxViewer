using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace EpxViewer
{
    public static class BehaviorHelper
    {
        public static void ApplyBehavior<T>(this DependencyObject d) where T : Behavior, new()
        {
            if (d == null) return;

            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(d);
            foreach (var behavior in itemBehaviors)
            {
                if ((behavior as T) == null) continue;
                itemBehaviors.Remove(behavior);
            }
            itemBehaviors.Add(new T());
        }
    }
}
