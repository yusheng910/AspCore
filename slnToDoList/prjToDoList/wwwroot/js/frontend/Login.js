var btnResetPwd = document.getElementById("resetPwd");
var input = document.getElementById("emailAddress");
var modalSubmitButton = document.getElementById("resetPwd");

btnResetPwd.onclick = function (e) {
    e.preventDefault();
    var mailAddress = input.value;

    modalSubmitButton.disabled = true;

    if (!CommonFn.validateEmail(mailAddress)) {
        alert("Invalid email format");
        modalSubmitButton.disabled = false;
        input.value = "";
        return;
    }

    $.ajax({
        url: "/api/UserResetPwdAPI",
        method: "POST",
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({ registeredMail: mailAddress }),
        success: function (response) {
            console.log(response);
            alert("New password has been sent to your mailbox");
            ResetModal(); 
        },
        error: function (e) {
            console.log("Failed in API: " + e.responseJSON.status);
            alert("Failed in reseting password");
            ResetModal(); 
        }
    })
}

function ResetModal() {
    $("#resetPwdModal").modal("hide");
    modalSubmitButton.disabled = false;
    input.value = "";
}