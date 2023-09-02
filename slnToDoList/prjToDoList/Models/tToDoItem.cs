using System;
using System.Collections.Generic;

namespace prjToDoList.Models;

public partial class tToDoItem
{
    public int fTaskId { get; set; }

    public int fUserId { get; set; }

    public string fTaskContent { get; set; } = null!;

    public bool fIsDone { get; set; }

    public DateTimeOffset fAddedDate { get; set; }

    public virtual tUser fUser { get; set; } = null!;
}
