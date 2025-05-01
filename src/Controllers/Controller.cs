using N8.Classes;
using N8.Utilities;
using N8.Lists;

namespace N8.Controllers
{
    public class Controller
    {
        public void Start(Target target)
        {
#pragma warning disable CS8604 // shouldnt be null once again for path, null is probably null
            TrackedFiles.FileList.Add(new FileNode(target.path, null));
#pragma warning restore CS8604 

            Console.WriteLine($"{ProcessUtilities.StartProcess(TrackedFiles.FileList[0])}");
            // TODO start ETW monitoring and tracking thy processes
            
            // idea is that controller will start etw monitoring and control everything listening to signals and stopping when all proc exited
            // will maintain list of monitored processes and files written to by any of these (to track if they are also spawned)
            
            // currently am able to spawn process correctly, as well as setup lists for monitoring
        }
    }
}
