// Config Bootstrap ToolTip
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

window.addEventListener("resize", function () {
    if (this.window.innerWidth <= 1200)
        HideoffcanvasMenu();
    else
        ShowoffcanvasMenu();
});

window.addEventListener("load", function () {
    if (this.window.innerWidth <= 1200)
        HideoffcanvasMenu();
    else
        ShowoffcanvasMenu();
});

function HideoffcanvasMenu() {
    document.getElementById("btnShowMenuToggler").classList.remove("visually-hidden");
    document.getElementById("offcanvasMenu").classList.remove("show");

    document.getElementById("menuBox-Temp").classList.remove("col-xxl-4");
    document.getElementById("mainBox").classList.remove("col-xxl-6");
    document.getElementById("mainBox").classList.add("col-xxl-11");

    document.getElementById("menuBox-Temp").classList.remove("col-lg-4");
    document.getElementById("mainBox").classList.remove("col-lg-11");
    document.getElementById("mainBox").classList.add("col-lg-11");
}

function ShowoffcanvasMenu() {
    document.getElementById("btnShowMenuToggler").classList.add("visually-hidden");
    document.getElementById("offcanvasMenu").classList.add("show");

    document.getElementById("menuBox-Temp").classList.add("col-xxl-4");
    document.getElementById("mainBox").classList.add("col-xxl-6");
    document.getElementById("mainBox").classList.remove("col-xxl-11");

    document.getElementById("menuBox-Temp").classList.add("col-lg-4");
    document.getElementById("mainBox").classList.add("col-lg-7");
    document.getElementById("mainBox").classList.remove("col-lg-11");
}

function ToggleSecondOffcanvasMenu() {
    document.getElementById("SecondOffcanvasMenu").classList.toggle("show");
}

// Function to handle click events on list items
function handleListItemClick(event) {
    // Get all list items with the 'userPanel-menuItem' class
    const listItems = document.querySelectorAll('.userPanel-menuItem');

    // Remove the 'userPanel-menuItem-active' class from all list items
    listItems.forEach(item => {
        item.classList.remove('userPanel-menuItem-active');
    });

    // Add the 'userPanel-menuItem-active' class to the clicked list item
    event.currentTarget.classList.add('userPanel-menuItem-active');
}

// Attach the click event listener to all list items with the 'userPanel-menuItem' class
function attachClickListeners() {
    const listItems = document.querySelectorAll('.userPanel-menuItem');

    listItems.forEach(item => {
        item.addEventListener('click', handleListItemClick);
    });
}

// Call the function to attach click listeners when the page is loaded
document.addEventListener('DOMContentLoaded', attachClickListeners);




// project section
function UpdateProjectsByOrganizationId(orgId = null, orgTitle = null) {

    if (orgId != null && orgTitle != null) {
        // Update the dropdown button text
        $('#ddlOrganization').text(orgTitle);
        $('#ddlOrganization').data("id", orgId);
    }
    else {
        orgId = $('#ddlOrganization').data('id');
    }

    // Change selection of menu items
    if (!$("#userPanel-menuItem-active").hasClass("userPanel-menuItem-active")) {
        document.getElementsByClassName("userPanel-menuItem")[0].classList.add("userPanel-menuItem-active");
        document.getElementsByClassName("userPanel-menuItem")[1].classList.remove("userPanel-menuItem-active");
        document.getElementsByClassName("userPanel-menuItem")[2].classList.remove("userPanel-menuItem-active");
    }

    $.ajax({
        url: "/Customer/UserPanel?handler=GetProjectsByOrganizationId",
        method: "GET",
        data: { organizationId: orgId },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            data.forEach(function (item) {
                var codeBlock = getProjectCard(item);
                list.append(codeBlock);
            });
            var addNewItemCard_codeBlock = getNewItemCard("#newProjectModal", "پروژه");
            list.append(addNewItemCard_codeBlock);
        },
        error: function () {
            alert("error");
        }
    });
}
function UpdateProjectsByFilter(parameter) {
    $.ajax({
        url: "/Customer/UserPanel?handler=GetProjectsByFilter",
        method: "GET",
        data: { filterParameter: parameter, orgId: $('#ddlOrganization').data("id") },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            data.forEach(function (item) {
                var codeBlock = getProjectCard(item);
                list.append(codeBlock);
            });
            var addNewItemCard_codeBlock = getNewItemCard("#newProjectModal", "پروژه");
            list.append(addNewItemCard_codeBlock);
        },
        error: function () {
            alert("error");
        }
    });
}
function updateProject(name, id) {
    // Update project input
    document.getElementById('txtProject_newTaskModal').value = name;
    document.getElementById('txtProject_newTaskModal').setAttribute('data-id', id);

    // Update users list
    $.ajax({
        url: "/Customer/UserPanel?handler=GetUsersByProjectId",
        method: "GET",
        data: { projectId: id },
        success: function (data) {
            var list = $('#usersList_newTaskModal');
            list.empty();
            data.forEach(function (item) {
                debugger;
                var codeBlock = `
                    <div class="form-check form-check-reverse d-flex align-items-center">
                        <input class="form-check-input bc-secondary hc-fs-paragraph2 ms-2" type="radio" value="${item.id}" name="Employees" id="employeeCheck${item.id}" onchange="updateAssignee('${item.fullName}', '${item.imageName}', '${item.id}')">
                        <label class="form-check-label w-100 hc-fs-paragraph3" for="employeeCheck${item.id}">
                            ${item.fullName}
                        </label>
                    </div>
                `;
                list.append(codeBlock);
            });
        },
        error: function () {
            alert("Error loading users.");
        }
    });
}
// end project section

// task section
function UpdateTasksByOrganizationId() {

    $.ajax({
        url: "/Customer/UserPanel?handler=GetTasksByOrganizationId",
        method: "GET",
        data: { organizationId: $("#ddlOrganization").data("id") },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            data.forEach(function (item) {
                var codeBlock = getTaskCard(item);
                list.append(codeBlock);
            });
            var addNewItemCard_codeBlock = getNewItemCard("#newTaskModal", "وظیفه");
            list.append(addNewItemCard_codeBlock);
        },
        error: function () {
            alert("error");
        }
    });
}
function UpdateTasksByFilter(parameter) {
    $.ajax({
        url: "/Customer/UserPanel?handler=GetTasksByFilter",
        method: "GET",
        data: { filterParameter: parameter, orgId: $('#ddlOrganization').data("id") },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            data.forEach(function (item) {
                var codeBlock = getTaskCard(item);
                list.append(codeBlock);
            });
            var addNewItemCard_codeBlock = getNewItemCard("#newTaskModal", "وظیفه");
            list.append(addNewItemCard_codeBlock);
        },
        error: function () {
            alert("error");
        }
    });
}
function updateAssignee(name, image, id) {
    document.getElementById('assigneeName').value = name;
    document.getElementById('assigneeImage').src = image;
    document.getElementById('assigneeName').setAttribute('data-id', id);
}
// end task section

// employee section
function UpdateEmployeesByOrganizationId() {

    $.ajax({
        url: "/Customer/UserPanel?handler=GetEmployeesByOrganizationId",
        method: "GET",
        data: { organizationId: $("#ddlOrganization").data("id") },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            data.forEach(function (item) {
                var codeBlock = getEmployeeCard(item);
                list.append(codeBlock);
            });
            var addNewItemCard_codeBlock = getNewItemCard("#newEmployeeModal", "همکار");
            list.append(addNewItemCard_codeBlock);
        },
        error: function () {
            alert("error");
        }
    });
}
function UpdateEmployeesByFilter(parameter) {
    $.ajax({
        url: "/Customer/UserPanel?handler=GetEmployeesByFilter",
        method: "GET",
        data: { filterParameter: parameter, orgId: $('#ddlOrganization').data("id") },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            data.forEach(function (item) {
                var codeBlock = getEmployeeCard(item);
                list.append(codeBlock);
            });
            var addNewItemCard_codeBlock = getNewItemCard("#newEmployeeModal", "همکار");
            list.append(addNewItemCard_codeBlock);
        },
        error: function () {
            alert("error");
        }
    });
}
function UpdateSentEmployeeInvitesByOrganizationId() {
    $.ajax({
        url: "/Customer/UserPanel?handler=GetSentEmployeeInvitesByOrganizationId",
        method: "GET",
        data: { organizationId: $("#ddlOrganization").data("id") },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            data.forEach(function (item) {
                var codeBlock = getSentEmployeeInviteCard(item);
                list.append(codeBlock);
            });
        },
        error: function () {
            alert("error");
        }
    });
}

// end employee section

function getProjectCard(item) {
    var codeBlock = `
                    <!-- item card -->
                                <div class="col-lg-4 col-md-4 col-sm-6 p-3">
                                    <div class="userPanel-itemCard hc-box bc-primary">
                                        <a href="#">
                                            <div class="d-flex flex-column justify-content-center align-items-center">
                                                <!-- item card image -->
                                                <img src="/src/media/Logo/regular-favicon-color.png" alt="item card image" width="65px"
                                                        height="65px" />
                                                <!-- item card title -->
                                                <h3 class="hc-fs-paragraph2 my-2">${item.title}</h3>
                                                <!-- item card tags -->
                                                <div class="itemCard-tagBox d-flex flex-column justify-content-center align-items-center py-3">
                                                    <span class="itemCard-tag p-1 px-3 bc-darkBlue rounded-3 text-white hc-fs-span3 my-1">
                                                        انجام شده : ${item.tasksStatusPercent}%
                                                    </span>
                                                    <span class="itemCard-tag p-1 px-3 bc-darkBlue rounded-3 text-white hc-fs-span3 my-1">
                                                        تعداد وظایف : ${item.tasksCount}
                                                    </span>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                </div>
                                <!-- end item card -->
                    `;
    return codeBlock;
}
function getTaskCard(item) {
    let priority = item.priority == '1' ? "bi-star" : item.priority == '2' ? "bi-star-half" : "bi-star-fill";
    var codeBlock = `
           <!-- item card -->
            <div class="col-lg-4 col-md-4 col-sm-6 p-3">
                            <div class="userPanel-itemCard hc-box bc-primary">
                            <a href="#">
                                <div class="d-flex flex-column justify-content-center align-items-center">
                                <!-- item card code -->
                                <div class="d-flex justify-content-between align-items-center w-100">
                                    <h3 class="hc-fs-paragraph1 my-2">${item.id}</h3>
                                    <i class="bi ${priority}"></i>
                                </div>

                                <!-- item card title -->
                                <h3 class="hc-fs-paragraph3 my-2">${item.title}</h3>
                                <!-- item card tags -->
                                <div class="itemCard-tagBox d-flex flex-column justify-content-center align-items-center py-3">
                                    <span class="itemCard-tag p-1 px-3 bc-darkBlue tc-lightBlue rounded-3 hc-fs-span3 my-1">
                                    پروژه : 
                                    ${item.project.title}
                                    </span>
                                    <span class="itemCard-tag p-1 px-3 bc-darkBlue text-warning rounded-3 hc-fs-span3 my-1">
                                    ${item.taskStatus}
                                    </span>
                                    <span class="itemCard-tag p-1 px-3 bc-darkBlue rounded-3 text-white hc-fs-span3 my-1">
                                    ${item.taskType}
                                    </span>
                                </div>
                                </div>
                            </a>
                            </div>
                        </div>
               <!-- end item card -->
     `;
    return codeBlock;
}
function getEmployeeCard(item) {
    var codeBlock = `
           <!-- item card -->
        <div class="col-lg-4 col-md-4 col-sm-6 p-3">
                      <div class="userPanel-itemCard hc-box bc-primary">
                        <a href="#">
                          <div class="d-flex flex-column justify-content-center align-items-center">
                            <!-- item card Image -->
                            <div class="d-flex justify-content-center align-items-center w-100">
                              <img src="${item.imageName}" width="100" height="100" alt="">
                            </div>

                            <!-- item card FullName -->
                            <h3 class="hc-fs-paragraph3 my-2">${item.fullName}</h3>

                            <!-- item card UserName -->
                            <h3 class="hc-fs-span1 my-2" dir="ltr">@${item.username}</h3>

                            <!-- item card tags -->
                            <div class="itemCard-tagBox d-flex flex-column justify-content-center align-items-center py-3">
                              <span class="itemCard-tag p-1 px-3 bc-darkBlue text-warning rounded-3 hc-fs-span3 my-1">
                                ${item.rank}
                              </span>
                              <span class="itemCard-tag p-1 px-3 bc-darkBlue rounded-3 text-white hc-fs-span3 my-1">
                                ${item.status}
                              </span>
                            </div>
                          </div>
                        </a>
                      </div>
                    </div>
               <!-- end item card -->
     `;
    return codeBlock;
}
function getSentEmployeeInviteCard(item) {
    let textColor = item.inviteStatus == 'پذیرفته شد' ? "text-success" : item.inviteStatus == 'پذیرفته نشد' ? "text-danger" : "text-warning";
    var codeBlock = `
           <!-- item card -->
        <div class="col-lg-4 col-md-4 col-sm-6 p-3">
                      <div class="userPanel-itemCard hc-box bc-primary">
                        <a href="#">
                          <div class="d-flex flex-column justify-content-center align-items-center">
                            <!-- item card Image -->
                            <div class="d-flex justify-content-center align-items-center w-100">
                              <img src="${item.imageName}" width="100" height="100" alt="">
                            </div>

                            <!-- item card FullName -->
                            <h3 class="hc-fs-paragraph3 my-2">${item.fullName}</h3>

                            <!-- item card UserName -->
                            <h3 class="hc-fs-span1 my-2" dir="ltr">@${item.username}</h3>

                            <!-- item card tags -->
                           <div class="itemCard-tagBox d-flex flex-column justify-content-center align-items-center py-3">
                            <span class="itemCard-tag p-1 px-3 bc-darkBlue ${textColor} rounded-3 hc-fs-paragraph2 fw-bold my-1">
                            ${item.inviteStatus}
                            </span>
                            </div>
                          </div>
                        </a>
                      </div>
                    </div>
               <!-- end item card -->
     `;
    return codeBlock;
}
function getNewItemCard(targetModal, categoryName) {
    var codeBlock = `
                    <!-- item card (new button) -->
                    <div class="col-lg-4 col-md-4 col-md-4 col-sm-6 p-3">
                        <div class="userPanel-itemCard hc-box bc-primary" data-bs-toggle="modal"
                                data-bs-target="${targetModal}">
                            <div class="d-flex flex-column justify-content-center align-items-center h-100">
                                <!-- item card icon -->
                                <i class="fa-solid fa-plus text-white hc-fs-title1" aria-hidden="true"></i>
                                <!-- item card title -->
                                <h3 class="hc-fs-paragraph2 my-2 text-center">
                                    افزودن ${categoryName} جدید
                                </h3>
                            </div>
                        </div>
                    </div>
                    <!-- end item card (new button) -->
                        `;
    return codeBlock;
}
function filterItems(parameter) {
    if (document.getElementsByClassName("userPanel-menuItem")[0].classList.contains("userPanel-menuItem-active"))  // projects tab selected
        UpdateProjectsByFilter(parameter);
    else if (document.getElementsByClassName("userPanel-menuItem")[1].classList.contains("userPanel-menuItem-active")) // tasks tab selected
        UpdateTasksByFilter(parameter);
    else if (document.getElementsByClassName("userPanel-menuItem")[2].classList.contains("userPanel-menuItem-active")) // employees tab selected
        UpdateEmployeesByFilter(parameter);
}