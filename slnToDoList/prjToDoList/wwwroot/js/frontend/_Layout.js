$.ajax({
    url: "/api/CheckLoginStatusAPI",
    method: "GET",
    success: function (response) {
        if (response.status == "login") {
            $("#navContainner").append(`
            <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between" id="navLst">
                <ul class="navbar-nav flex-grow-1">
                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/User/Edit">Edit User Profile</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/Home/Logout">Log out</a>
                    </li>
                </ul>
            </div>
            `);
        }
    },
    error: function (e) {
        $("#navLst").remove();
        console.log("Failed in API", e);
    }
})