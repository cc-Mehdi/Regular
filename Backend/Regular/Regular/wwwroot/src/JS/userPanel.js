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


// organization section
function UpdateOrganizationsByLoggedInUserId() {

    $.ajax({
        url: "/Customer/UserPanel?handler=GetOrganizationsByLoggedInUserId",
        method: "GET",
        data: { organizationId: $("#ddlOrganization").data("id") },
        success: function (data) {
            var list = $('#organizationsList');
            list.empty();
            data.forEach(function (item) {
                var codeBlock = getOrganizationItems(item);
                list.append(codeBlock);
            });
            if (data.length > 0)
                UpdateProjectsByOrganizationId(data[0].id, data[0].title);
            else {
                UpdateProjectsByOrganizationId(0);
                $('#ddlOrganization').text("سازمان ها");
            }
        },
        error: function () {
            alert("error");
        }
    });
}

// end organization section

// project section
function UpdateProjectsByOrganizationId(orgId = null, orgTitle = null) {
    debugger;
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
        document.getElementsByClassName("userPanel-menuItem")[3].classList.remove("userPanel-menuItem-active");
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

    updateProjectsList_newTaskModal(orgId);
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
function updateProjectsList_newTaskModal(orgId) {
    $.ajax({
        url: "/Customer/UserPanel?handler=GetProjectsByOrganizationId",
        method: "GET",
        data: { organizationId: orgId },
        success: function (data) {
            var newTaskModal_projectsList = $('#newTaskModal_projectsList');
            newTaskModal_projectsList.empty();
            data.forEach(function (item) {
                var codeBlock = `
                <div class="form-check form-check-reverse d-flex align-items-center">
                    <input class="form-check-input bc-secondary hc-fs-paragraph2 ms-2"
                            type="radio"
                            value="${item.id}"
                            name="project"
                            id="projectCheck@item.Id"
                            onchange="updateProject('${item.title}', '${item.id}')">
                    <label class="form-check-label w-100 hc-fs-paragraph3" for="projectCheck@item.Id">
                        ${item.title}
                    </label>
                </div>
                `;
                newTaskModal_projectsList.append(codeBlock);
            });
        },
        error: function () {
            alert("error");
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
function UpdateTasksByProjectId(projectId) {
    // Change selection of menu items
    document.getElementsByClassName("userPanel-menuItem")[0].classList.remove("userPanel-menuItem-active");
    document.getElementsByClassName("userPanel-menuItem")[1].classList.add("userPanel-menuItem-active");
    document.getElementsByClassName("userPanel-menuItem")[2].classList.remove("userPanel-menuItem-active");
    document.getElementsByClassName("userPanel-menuItem")[3].classList.remove("userPanel-menuItem-active");

    // Update tasks list
    $.ajax({
        url: "/Customer/UserPanel?handler=GetTasksByProjectId",
        method: "GET",
        data: { projectId: projectId },
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
            alert("مشکل در نمایش وظایف");
        }
    });
}
function ShowTaskDetails(taskId) {
    // get task
    $.ajax({
        url: "/Customer/UserPanel?handler=GetTaskById",
        method: "GET",
        data: { taskId: taskId },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            var codeBlock = getTaskDetailsCard(data);
            list.append(codeBlock);
        },
        error: function () {
            alert("مشکل در نمایش وظایف");
        }
    });
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
function showUserInformationByUserId(userId) {
    // Update tasks list
    $.ajax({
        url: "/Customer/UserPanel?handler=GetUserById",
        method: "GET",
        data: { userId: userId },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            var codeBlock = getUserInformationCard(data);
            list.append(codeBlock);
        },
        error: function () {
            alert("مشکل در نمایش مشخصات کاربر انتخاب شده");
        }
    });
}
// end employee section

function getProjectCard(item) {
    var codeBlock = `
                    <!-- item card -->
                                <div class="col-lg-4 col-md-4 col-sm-6 p-3">
                                    <div class="userPanel-itemCard hc-box bc-primary">
                                        <a>
                                            <div class="d-flex flex-column justify-content-center align-items-center">
                                                <!-- item card image -->
                                                <img src="/src/media/Logo/regular-favicon-color.png" alt="item card image" width="65px"
                                                        height="65px" />
                                                <!-- item card title -->
                                                <h3 class="hc-fs-paragraph2 my-2" onclick="UpdateTasksByProjectId(${item.id})">
                                                    <span class="itemCard-title">${item.title}</span>
                                                </h3>
                                                <!-- item card tags -->
                                                <div class="itemCard-tagBox d-flex flex-column justify-content-center align-items-center py-3">
                                                    <span class="itemCard-tag p-1 px-3 bc-darkBlue rounded-3 text-white hc-fs-span3 my-1">
                                                        انجام شده : ${item.tasksStatusPercent}%
                                                    </span>
                                                    <span class="itemCard-tag p-1 px-3 bc-darkBlue rounded-3 text-white hc-fs-span3 my-1">
                                                        تعداد وظایف : ${item.tasksCount}
                                                    </span>
                                                </div>
                                                <!-- item card buttons -->
                                                <div class="row w-100">
                                                  <div class="col-sm-12 col-md-6 px-1">
                                                    <button class="btn bg-warning w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                                                      <i class="bi bi-pencil-square ms-1"></i>
                                                      <span>ویرایش</span>
                                                    </button>
                                                  </div>
                                                  <div class="col-sm-12 col-md-6 px-1">
                                                    <button onclick="DeleteItem('Project', ${item.id})" class="btn bg-danger w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                                                      <i class="bi bi-trash ms-1"></i>
                                                      <span>حذف</span>
                                                    </button>
                                                  </div>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                </div>
                                <!-- end item card -->
                    `;
    return codeBlock;
}
function getOrganizationItems(item) {
    var codeBlock = `
           <li class="d-flex flex-column align-items-center">
                <a class="dropdown-item text-end text-white py-3" onclick="UpdateProjectsByOrganizationId('${item.id}', '${item.title}')">
                    <span>
                        ${item.title}
                    </span>
                </a>
                <div class="row w-100">
                    <div class="col-sm-12 col-md-6 px-1">
                        <button class="btn bg-warning w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                            <i class="bi bi-pencil-square ms-1"></i>
                            <span>ویرایش</span>
                        </button>
                    </div>
                    <div class="col-sm-12 col-md-6 px-1">
                        <button onclick="DeleteItem('Organization', ${item.id})" class="btn bg-danger w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                            <i class="bi bi-trash ms-1"></i>
                            <span>حذف</span>
                        </button>
                    </div>
                </div>
            </li>
     `;
    return codeBlock;
}
function getTaskCard(item) {
    let priority = item.priority == '1' ? "bi-star" : item.priority == '2' ? "bi-star-half" : "bi-star-fill";
    var codeBlock = `
           <!-- item card -->
            <div class="col-lg-4 col-md-4 col-sm-6 p-3">
                            <div class="userPanel-itemCard hc-box bc-primary">
                            <a onclick="ShowTaskDetails(${item.id})">
                                <div class="d-flex flex-column justify-content-center align-items-center">
                                <!-- item card code -->
                                <div class="d-flex justify-content-between align-items-center w-100">
                                    <h3 class="hc-fs-paragraph1 my-2">${item.id}</h3>
                                    <i class="bi ${priority}"></i>
                                </div>

                                <!-- item card title -->
                                <h3 class="hc-fs-paragraph2 my-2" onclick="UpdateTasksByProjectId(${item.id})">
                                    <span class="itemCard-title">${item.title}</span>
                                </h3>
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

                                <!-- item card buttons -->
                                <div class="row w-100">
                                  <div class="col-sm-12 col-md-6 px-1">
                                    <button class="btn bg-warning w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                                      <i class="bi bi-pencil-square ms-1"></i>
                                      <span>ویرایش</span>
                                    </button>
                                  </div>
                                  <div class="col-sm-12 col-md-6 px-1">
                                    <button onclick="DeleteItem('Task', ${item.id})" class="btn bg-danger w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                                      <i class="bi bi-trash ms-1"></i>
                                      <span>حذف</span>
                                    </button>
                                  </div>
                                </div>

                                </div>
                            </a>
                            </div>
                        </div>
               <!-- end item card -->
     `;
    return codeBlock;
}
function getTaskDetailsCard(item) {
    let priorityIcon = item.priority == '1' ? "bi-star" : item.priority == '2' ? "bi-star-half" : "bi-star-fill";
    let priorityValue = item.priority == '1' ? "کم" : item.priority == '2' ? "متوسط" : "زیاد";
    var codeBlock = `
            <!-- Task details card -->
            <div class="w-100 h-100 d-flex justify-content-center align-items-center mt-3">
            <!-- back button -->
              <div class="row w-100 d-flex flex-row-reverse">
                <button onclick="UpdateTasksByOrganizationId()" class="btn btn-outline-primary w-25 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                  <i class="bi bi-arrow-left-square-fill tc-lightCyan hc-fs-paragraph1 ms-2"></i>
                  <span class="hc-fs-paragraph2">بازگشت</span>
                </button>
              <div class="taskDetailBox bc-primary w-100 rounded-3 d-flex flex-column p-5 px-3">
                <div class="w-100 d-flex justify-content-between">
                  <h5 class="hc-fs-paragraph3">${item.id}</h5>
                  <h5 class="hc-fs-paragraph3">${item.project.title}</h5>
                </div>
                <div class="w-100">
                  <h5 class="hc-fs-title2">${item.title}</h5>
                </div>
                <div class="w-100 d-flex flex-column justify-content-center align-items-center mt-5">
                  <div class="row w-100 my-2">
                    <div class="col-sm-6 col-12">
                      <span class="hc-fs-paragraph3 tc-lightCyan">مرتبط با : </span>
                      <span class="hc-fs-paragraph2 fw-bold text-white">${item.assignto}</span>
                    </div>
                    <div class="col-sm-6 col-12">
                      <span class="hc-fs-paragraph3 tc-lightCyan">اولویت : </span>
                      <span class="hc-fs-paragraph2 fw-bold text-white">
                        <i class="bi ${priorityIcon} hc-fs-span1"></i>
                        ${priorityValue}
                      </span>
                    </div>
                  </div>
                  <div class="row w-100 my-2">
                    <div class="col-sm-6 col-12">
                      <span class="hc-fs-paragraph3 tc-lightCyan">وضعیت : </span>
                      <span class="hc-fs-paragraph2 fw-bold text-white">${item.taskStatus}</span>
                    </div>
                    <div class="col-sm-6 col-12">
                      <span class="hc-fs-paragraph3 tc-lightCyan">نوع : </span>
                      <span class="hc-fs-paragraph2 fw-bold text-white">
                        ${item.taskType}
                      </span>
                    </div>
                  </div>
                  <div class="row w-100 my-2">
                    <div class="col-sm-6 col-12">
                      <span class="hc-fs-paragraph3 tc-lightCyan">زمان تخمینی : </span>
                      <span class="hc-fs-paragraph2 fw-bold text-white">${item.estimateTime}</span>
                    </div>
                    <div class="col-sm-6 col-12">
                      <span class="hc-fs-paragraph3 tc-lightCyan">زمان ثبت شده : </span>
                      <span class="hc-fs-paragraph2 fw-bold text-white">
                        ${item.loggedTime}
                      </span>
                    </div>
                  </div>
                  <div class="row w-100 my-2">
                    <div class="col">
                      <span class="hc-fs-paragraph3 tc-lightCyan">توضیحات : </span>
                      <span class="hc-fs-paragraph2 fw-bold text-white">
                        ${item.description}
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <!-- End Task details card -->
     `;
    return codeBlock;
}
function getEmployeeCard(item) {
    var codeBlock = `
           <!-- item card -->
        <div class="col-lg-4 col-md-4 col-sm-6 p-3">
                      <div class="userPanel-itemCard hc-box bc-primary" style="height: 350px;">
                        <a>
                          <div class="d-flex flex-column justify-content-center align-items-center">
                            <!-- item card Image -->
                            <div class="d-flex justify-content-center align-items-center w-100">
                              <img src="${item.imageName}" width="100" height="100" alt="">
                            </div>

                            <!-- item card FullName -->
                            <h3 class="hc-fs-paragraph2 my-2" onclick="showUserInformationByUserId(${item.id})">
                                <span class="itemCard-title">${item.fullName}</span>
                            </h3>

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

                            <!-- item card buttons -->
                                <div class="row w-100">
                                  <div class="col-sm-12 col-md-6 px-1">
                                    <button class="btn bg-warning w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                                      <i class="bi bi-pencil-square ms-1"></i>
                                      <span>ویرایش</span>
                                    </button>
                                  </div>
                                  <div class="col-sm-12 col-md-6 px-1">
                                    <button onclick="DeleteItem('Employee', ${item.id})" class="btn bg-danger w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                                      <i class="bi bi-trash ms-1"></i>
                                      <span class="text-nowrap">لغو همکاری</span>
                                    </button>
                                  </div>
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
                        <a onclick="showUserInformationByUserId(${item.id})">
                          <div class="d-flex flex-column justify-content-center align-items-center">
                            <!-- item card Image -->
                            <div class="d-flex justify-content-center align-items-center w-100">
                              <img src="${item.imageName}" width="100" height="100" alt="">
                            </div>

                            <!-- item card FullName -->
                            <h3 class="hc-fs-paragraph2 my-2" onclick="UpdateTasksByProjectId(${item.id})">
                                <span class="itemCard-title">${item.fullName}</span>
                            </h3>

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
function getUserInformationCard(item) {
    var codeBlock = `
<!-- User Profile card -->
<div class="w-100 h-100 d-flex flex-column justify-content-center align-items-center mt-3">
    <!-- back button -->
    <div class="row w-100 d-flex flex-row-reverse">
        <button onclick="UpdateEmployeesByOrganizationId()" class="btn btn-outline-primary w-25 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
            <i class="bi bi-arrow-left-square-fill tc-lightCyan hc-fs-paragraph1 ms-2"></i>
            <span class="hc-fs-paragraph2">بازگشت</span>
        </button>
    </div>
    <div class="bc-primary w-100 rounded-3 d-flex flex-column p-5 px-3">
        <!-- image and main info -->
        <div class="w-100 d-flex justify-content-end align-items-center row">
            <div class="col-md-4 col-sm-12  text-center">
                <img src="${item.imageName}"
                     style="max-width: 200px; min-width: 100px; width: 100%;" alt="user profile image">
            </div>
            <div class="col-md-8 col-sm-12 d-flex flex-column py-3 px-1 text-center">
                <h5 class="hc-fs-title3">${item.fullName}</h5>
                <h5 class="hc-fs-title3">@${item.username}</h5>
                <h5 class="hc-fs-title3">${item.rank}</h5>
                <h5 class="hc-fs-title3">${item.status}</h5>
            </div>
        </div>

    </div>
</div>
<!-- End User Profile card -->
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

function DeleteItem(sectionName, itemId) {
    let customUrl = "";
    switch (sectionName) {
        case "Organization":
            customUrl = "/Customer/UserPanel?handler=DeleteOrganization";
            break;
        case "Project":
            customUrl = "/Customer/UserPanel?handler=DeleteProject";
            break;
        case "Task":
            customUrl = "/Customer/UserPanel?handler=DeleteTask";
            break;
        case "Employee":
            customUrl = "/Customer/UserPanel?handler=DeleteEmployee";
            break;
        default:
    }

    // Update list
    $.ajax({
        url: customUrl,
        method: "GET",
        data: { id: itemId },
        success: function (data) {
            if (data.errorMessage != "") {
                toastr.error(data.errorMessage, 'خطا');
                return;
            }
            switch (sectionName) {
                case "Organization":
                    UpdateOrganizationsByLoggedInUserId();
                    toastr.success("حذف سازمان با موفقیت انجام شد", 'موفق');
                    break;
                case "Project":
                    UpdateProjectsByOrganizationId();
                    toastr.success("حذف پروژه با موفقیت انجام شد", 'موفق');
                    break;
                case "Task":
                    UpdateTasksByOrganizationId();
                    toastr.success("حذف وظیفه با موفقیت انجام شد", 'موفق');
                    break;
                case "Employee":
                    UpdateEmployeesByOrganizationId();
                    toastr.success("لغو همکاری با موفقیت انجام شد", 'موفق');
                    break;
                default:
            }
        },
        error: function () {
            toastr.error("انجام عملیات با شکست مواجه شد", 'خطا');
        }
    });
}