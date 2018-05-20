using System.Collections.Generic;
using System.Linq;

namespace SmartHotel.Services.Task.Data.Seeder
{
    public class TaskSeeder
    {
        public static void Seed(TaskDbContext db)
        {
            if (db.Tasks.Any()) return;

            var statuses = new List<string>();
            statuses.Add("pending");
            statuses.Add("resolved");

            var taskTypes = new Dictionary<int, string>();
            taskTypes.Add(5, "changeTowels");
            taskTypes.Add(4, "cleanRoom");
            taskTypes.Add(3, "newGuest");
            taskTypes.Add(2, "roomService");
            taskTypes.Add(1, "airConditioner");

            var tasks = new List<TaskAndType>();
            tasks.Add(new TaskAndType(1, "AC is stuck on, I have played with the dial and no matter what I do it keeps blowing cold air."));
            tasks.Add(new TaskAndType(1, "My AC unit has a weird smell coming from it. Smells like mold."));
            tasks.Add(new TaskAndType(1, "The AC in my room is very loud, sounds like a lawn mower is running in my room."));
            tasks.Add(new TaskAndType(1, "Air conditioning is not working, no power, no fan... Nothing."));
            tasks.Add(new TaskAndType(4, "Room service forgot my drinks, I ordered a fizzy drink and grape juice"));
            tasks.Add(new TaskAndType(4, "I ordered my breakfast over an hour ago... where is my pancake...????"));
            tasks.Add(new TaskAndType(4, "My kids ordered a bunch of food through the TV, and I need to cancel my room orders. ASAP"));
            tasks.Add(new TaskAndType(3, "I lost my room key already, I need a replacement key"));
            tasks.Add(new TaskAndType(3, "I want a west facing window, very important for my sleep cycle, please move me."));
            tasks.Add(new TaskAndType(3, "Can I get a daily newspaper on my door each morning?"));
            tasks.Add(new TaskAndType(3, "Deliver a fold out bed for one of my kids."));
            tasks.Add(new TaskAndType(5, "Please come by my room this afternoon and replace towels and mats"));
            tasks.Add(new TaskAndType(2, "Vacuum room and wipe down the counters, it seems dusty in here"));
            tasks.Add(new TaskAndType(2, "Sterilize the room with bleach please, I am germ-phobic..."));
            tasks.Add(new TaskAndType(2, "Clean toilet, shower, and sink"));

            for (int i = 0; i < tasks.Count; i++)
            {
                var task = new Task.Domain.Task();
                task.UserId = "bwaiters@smarthotel360.com";
                task.Rooom = (i + 10) * 4;
                var taskAndType = tasks[i];
                task.TaskType = taskAndType.Type;
                task.Resolved = false;
                task.Description = taskAndType.Task;
                db.Tasks.Add(task);
            }

            db.SaveChanges();
        }
    }

    public class TaskAndType
    {
        public int Type { get; }
        public string Task { get; }
        public TaskAndType(int type, string task)
        {
            this.Task = task;
            this.Type = type;
        }
    }
}