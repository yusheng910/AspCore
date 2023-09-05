$(".list-group-item").hover(
    function () {
        $(this).addClass("highlight");
    },
    function () {
        $(this).removeClass("highlight");
    }
);

$(".addedDate").each(function (index, element) {
    var dateStringStored = $(element).text();
    var dateStored = new Date(dateStringStored);
    var dateShown = dateStored.toLocaleDateString('zh-TW', { year: 'numeric', month: '2-digit', day: '2-digit' }).replace(/\//g, '-');

    $(element).text(`Created on ${dateShown}`);
});

var select = $(".select");
select.on("change", function () {
    var selectedValue = $(this).val();
    var ulElements = $("ul[data-taskid]");

    if (selectedValue === "2") {
        ulElements.each(function () {
            var todoContent = $(this).find(".todocontent");
            if (!todoContent.hasClass("crossout")) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    } else if (selectedValue === "3") {
        ulElements.each(function () {
            var todoContent = $(this).find(".todocontent");
            if (todoContent.hasClass("crossout")) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    } else {
        // selectedValue === "All"
        ulElements.show();
    }
});

// Change task Status function
$("#todoContainer").on("click", ".todocontent", function () {
    var $this = $(this); // save $(this) variable so it can be used in ajax call
    var taskId = $this.closest("ul").data("taskid");
    $.ajax({
        url: "/api/ToDoAPI/" + taskId,
        type: "PUT",
        success: function (response) {
            console.log(response);

            // 更新成功後，改變項目的顯示狀態
            if (response.isDone) {
                $this.addClass("crossout");
                setTimeout(function () {
                    if ($(".select").val() == 3) {
                        $this.closest("ul").hide();
                    }
                }, 300);
            } else {
                $this.removeClass("crossout");
                setTimeout(function () {
                    if ($(".select").val() == 2) {
                        $this.closest("ul").hide();
                    }
                }, 300);
            }
        },
        error: function (e) {
            console.log("Error: " + e.responseJSON.status);
            alert("Failed in updating task status");
        }
    });

    //if ($(this).hasClass("crossout")) {
    //    $(this).removeClass("crossout");
    //} else {
    //    $(this).addClass("crossout");
    //}
});

// Delete task function
$("#todoContainer").on("click", ".delete-button", function () {
    var confirmation = confirm("Do you wanna delete this task？");
    if (confirmation) {
        var $this = $(this); // save $(this) variable so it can be used in ajax call
        var taskId = $this.closest("ul").data("taskid");

        console.log(taskId)

        $.ajax({
            url: "/api/ToDoAPI/" + taskId,
            type: "DELETE",
            success: function (response) {
                console.log(response);
                $this.closest('ul').remove();
            },
            error: function (e) {
                console.log("Error: " + e.responseJSON.status);
                alert("Failed in deleting task");
            }
        });
    }
});

// Add task function
$("#add-button").on("click", function (e) {
    e.preventDefault();
    var $addContent = $("#add-input").val();

    if ($addContent == "") {
        alert("Forget something to add?")

    } else {
        $("#add-button").prop('disabled', true);
        $.ajax({
            url: "/api/ToDoAPI",
            method: "POST",
            headers: { "content-type": "application/x-www-form-urlencoded" },
            data: {
                taskContent: $addContent
            },
            success: function (response) {
                console.log(response.addedTime);
                RenderAndReset($addContent, response.addedTime, response.taskId);
            },
            error: function (e) {
                console.log("Failed in API: " + e.responseJSON.status);
                alert("Failed in adding task");

            }
        })
    }
})

function RenderAndReset(taskContent, addedDate, taskId) {

    var dateStored = new Date(addedDate)
    var dateShown = dateStored.toLocaleDateString('zh-TW', { year: 'numeric', month: '2-digit', day: '2-digit' }).replace(/\//g, '-');
    // reset select to all once new task added 
    $(".select").val("1").trigger("change");
    $("#todoContainer").append(`                                
            <ul class="list-group list-group-horizontal rounded-0 bg-transparent" data-taskid="${taskId}">
                <li class="list-group-item d-flex align-items-center ps-0 pe-3 py-1 rounded-0 border-0 bg-transparent serialnum">
                    *
                </li>
                <li class="list-group-item px-3 py-1 d-flex align-items-center flex-grow-1 border-0 bg-transparent todocontent">
                    <p class="lead fw-normal mb-0">${taskContent}</p>
                </li>
                <li class="list-group-item ps-3 pe-0 py-1 rounded-0 border-0 bg-transparent">
                    <div class="d-flex flex-row justify-content-end mb-1">
                        <a href="#!" class="text-danger delete-button" data-mdb-toggle="tooltip" title="Delete todo">
                            <i class="fas fa-trash-alt"></i>
                        </a>
                    </div>
                    <p class="small mb-0 addedDate">Created on ${dateShown}</p>
                </li>
            </ul>            
        `);
    $("#add-input").val("");
    $("#add-button").prop('disabled', false);
}