using System;
using System.Collections.Generic;

namespace prjToDoList.Models;

public partial class tUser
{
    public int fUserId { get; set; }

    public string fUserName { get; set; } = null!;

    public string fPassword { get; set; } = null!;

    public string fEmail { get; set; } = null!;

    public string? fMobile { get; set; }

    public virtual ICollection<tToDoItem> tToDoItems { get; set; } = new List<tToDoItem>();
}
