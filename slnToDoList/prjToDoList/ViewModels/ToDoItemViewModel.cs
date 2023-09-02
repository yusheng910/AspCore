using prjToDoList.Models;

namespace prjToDoList.ViewModels
{
    public class ToDoItemViewModel
    {
        public ToDoItemViewModel() { ToDoItem = new tToDoItem(); fAddedDate = string.Empty; }
        public tToDoItem ToDoItem { get; set; }
        public int fTaskId
        {
            get { return this.ToDoItem.fTaskId; }
            set { this.ToDoItem.fTaskId = value; }
        }

        public int fUserId
        {
            get { return this.ToDoItem.fUserId; }
            set { this.ToDoItem.fUserId = value; }
        }

        public string fTaskContent
        {
            get { return this.ToDoItem.fTaskContent; }
            set { this.ToDoItem.fTaskContent = value; }
        }

        public bool fIsDone
        {
            get { return this.ToDoItem.fIsDone; }
            set { this.ToDoItem.fIsDone = value; }
        }

        public string fAddedDate
        {
            get;
            set;
        }
    }
}
