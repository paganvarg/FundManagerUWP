using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace FundManagerUWP.Helpers
{
    public static class BackgroundTaskRegistrationHelper
    {
        public static IBackgroundTaskRegistration RegisterBackgroundTask(Type taskEntryPoint,
            string taskName,
            IBackgroundTrigger trigger,
            IBackgroundCondition condition)
        {
            //
            // Check for existing registrations of this background task.
            //

            var existingTask = BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(bt => bt.Name == taskName);
            if (existingTask != null)
            {
                existingTask.Unregister(true);
            }

            //
            // Register the background task.
            //

            var builder = new BackgroundTaskBuilder();

            builder.Name = taskName;
            builder.TaskEntryPoint = taskEntryPoint.FullName;
            builder.SetTrigger(trigger);

            if (condition != null)
            {

                builder.AddCondition(condition);
            }

            return builder.Register();
        }
    }
}
