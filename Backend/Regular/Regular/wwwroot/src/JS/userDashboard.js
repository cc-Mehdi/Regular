let accountUserImageInput = document.createElement('input');
var accountUserImage = null;

// Update requests list in every 30 seconds
setInterval(ShowRequestsCount, 60000);

ShowAccountTab();

// save username to clipboard
function SaveToClipboard(tag) {
    let textContent = tag;

    // Create a temporary textarea element to hold the text to be copied
    const tempTextArea = document.createElement('textarea');
    tempTextArea.value = textContent.innerHTML;
    document.body.appendChild(tempTextArea);

    // Select the text and copy it to the clipboard
    tempTextArea.select();
    document.execCommand('copy');

    // Remove the temporary textarea
    document.body.removeChild(tempTextArea);

    // Show success message using Toastr
    toastr.success('نام کاربری کپی شد');
}

// edit account control
function onEdit(inputIndex) {
    switch (inputIndex) {
        case 1: // edit image
            accountUserImageInput.type = 'file';
            accountUserImageInput.click();
            break;
        case 2: // edit fullname

            break;
        case 3: // edit rank
            break;
        case 4: // edit status
            break;
        case 5: // edit email
            break;
        case 6: // edit pass
            break;
    }
}

accountUserImageInput.addEventListener('change', function (e) {
    if (e.target.files[0]) {
        var file = e.target.files[0];
        var fileType = file.type;
        var fileSize = file.size;
        var allowedTypes = ['image/png', 'image/jpeg', 'image/gif'];
        var maxSizeInBytes = 25 * 1024 * 1024; // 25 MB in bytes

        if (!allowedTypes.includes(fileType)) {
            toastr.warning("لطفا فایل انتخابی تصویر یا گیف باشد (.png | .jpg | .gif)");
        } else if (fileSize > maxSizeInBytes) {
            toastr.warning("حجم فایل بیش از حد مجاز (25MB)");
        } else {
            accountUserImage = file;
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#imgAccountUserImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    }
});


// account inputs control
var sectionOne;
var sectionTwo;
function toggleSections(sectionOneId, sectionTwoId) {

    document.getElementById("userInfo-txtFullname").classList.add("d-none");
    document.getElementById("userInfo-lblFullname").classList.remove("d-none");

    document.getElementById("userInfo-txtRank").classList.add("d-none");
    document.getElementById("userInfo-lblRank").classList.remove("d-none");

    document.getElementById("userInfo-txtStatus").classList.add("d-none");
    document.getElementById("userInfo-lblStatus").classList.remove("d-none");

    document.getElementById("userInfo-txtEmail").classList.add("d-none");
    document.getElementById("userInfo-lblEmail").classList.remove("d-none");

    document.getElementById("userInfo-txtPass").classList.add("d-none");
    document.getElementById("userInfo-lblPass").classList.remove("d-none");


    sectionOne = document.getElementById(sectionOneId);
    sectionTwo = document.getElementById(sectionTwoId);
    if (sectionTwo.classList.contains('d-none')) {
        sectionOne.classList.add('d-none');
        sectionTwo.classList.remove('d-none');
        sectionTwo.children[0].value = sectionOne.innerHTML;
    } else {
        sectionOne.classList.remove('d-none');
        sectionTwo.classList.add('d-none');
        sectionOne.innerHTML = sectionTwo.children[0].value;
    }
}
document.addEventListener('click', function (event) {
    if (sectionTwo != undefined && sectionOne != undefined) {
        if (!sectionTwo.contains(event.target) && !sectionOne.contains(event.target)) {
            sectionOne.classList.remove('d-none');
            sectionTwo.classList.add('d-none');
            sectionOne.innerHTML = sectionTwo.children[0].value;
        }
    }
});
// end account inputs control


// tab controls
function ShowAccountTab() {

    $.ajax({
        url: "/Customer/UserDashboard?handler=GetLoggedInUser",
        method: "GET",
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            var codeBlock = getAccountCard(data);
            list.append(codeBlock);

            // show request count to badge
            ShowRequestsCount();
        },
        error: function () {
            toastr.error("در لود صفحه خطا وجود دارد", "خطا");
        }
    });
}
function ShowRelationTab() {

    $.ajax({
        url: "/Customer/UserDashboard?handler=GetRelationOrganizations",
        method: "GET",
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            var codeBlock = '';
            data.forEach(function (item) {
                codeBlock += getRelationCard(item);
            });
            list.append(GetCardHead(`ShowAccountTab()`, "همکاری ها") + codeBlock + GetCardFoot());

        },
        error: function () {
            toastr.error("در لود صفحه خطا وجود دارد", "خطا");
        }
    });
}
function ShowRequestTab() {

    $.ajax({
        url: "/Customer/UserDashboard?handler=GetSentEmployeeInvites",
        method: "GET",
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            var codeBlock = '';
            data.forEach(function (item) {
                codeBlock += getRequestCard(item);
            });
            list.append(GetCardHead(`ShowAccountTab()`, "درخواست ها") + codeBlock + GetCardFoot());

            // show request count to badge
            ShowRequestsCount();
        },
        error: function () {
            toastr.error("در لود صفحه خطا وجود دارد", "خطا");
        }
    });
}
function ShowReportsTab() {

    $.ajax({
        url: "/",
        method: "GET",
        data: {},
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            var head = `
                <!-- item card -->
                <div class="w-100 bc-primary rounded-3 p-3 d-flex flex-column">
                <!-- label -->
                <span class="app-brand-text demo menu-text text-white fs-4 fw-bold me-2" style="text-shadow: 0 0 1px var(--c-primary)">گزارشات</span>
                <hr class="text-white">
              `;
            var foot = `
                </div>
                <!-- end item card -->
            `;
            var codeBlock = head + getReportsCard(data) + foot;
            list.append(codeBlock);
        },
        error: function () {
            toastr.error("در لود صفحه خطا وجود دارد", "خطا");
        }
    });
}
function ShowProjectsByOrgId(orgId) {
    $.ajax({
        url: "/Customer/UserPanel?handler=GetProjectsByOrganizationId",
        method: "GET",
        data: { organizationId: orgId },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            var codeBlock = '';
            data.forEach(function (item) {
                codeBlock += getProjectCard(item);
            });
            list.append(GetCardHead(`ShowRelationTab()`, "پروژه ها") + codeBlock + GetCardFoot());
        },
        error: function () {
            toastr.error("در لود صفحه خطا وجود دارد", "خطا");
        }
    });
}
function ShowTasksByProjectId(projectId) {
    $.ajax({
        url: "/Customer/UserPanel?handler=GetTasksByProjectId",
        method: "GET",
        data: { projectId: projectId },
        success: function (data) {
            var list = $('#itemsList');
            list.empty();
            var codeBlock = '';
            data.forEach(function (item) {
                codeBlock += getTaskCard(item);
            });
            list.append(GetCardHead(`ShowProjectsByOrgId(${projectId})`, "وظایف") + codeBlock + GetCardFoot());
        },
        error: function () {
            toastr.error("در لود صفحه خطا وجود دارد", "خطا");
        }
    });
}
function ShowRequestsCount() {
    $.ajax({
        url: "/Customer/UserDashboard?handler=GetSentEmployeeInvitesCount",
        method: "GET",
        success: function (data) {
            if (data.count > 0) {
                // show request count to badge
                $("#lblRequestCountBadge").html(data.count);
                $("#lblRequestCountBadge").removeAttr("hidden");
            }
            else
                $("#lblRequestCountBadge").attr("hidden", "true");
        },
        error: function () {
            toastr.error("در لود صفحه خطا وجود دارد", "خطا");
        }
    });
}
// end tab controls

// module cards
function GetCardHead(backFunction, title) {
    return `
                <!-- back button -->
                <div class="row w-100 d-flex flex-row-reverse">
                <button onclick="{${backFunction}}" class="btn btn-outline-primary col-12 col-sm-6 col-md-4 col-xl-3 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                    <i class="bi bi-arrow-left-square-fill tc-lightCyan hc-fs-paragraph1 ms-2"></i>
                    <span class="hc-fs-paragraph2">بازگشت</span>
                </button>
                </div>
                <!-- item card -->
                <div class="w-100 bc-primary rounded-3 p-3 d-flex flex-column">
                <!-- label -->
                <span class="app-brand-text demo menu-text text-white fs-4 fw-bold me-2" style="text-shadow: 0 0 1px var(--c-primary)">${title} </span>
                <hr class="text-white">

                <!-- Search Box -->
                <input type="search" class="form-control border-0 tc-lightBlue fs-5 py-2 w-100" placeholder="جستجو">

                <div id="itemsList" class="row w-100 d-flex justify-content-center mt-3">
              `;
}
function GetCardFoot() {
    return `
        </div>
        </div>
        <!-- end item card -->
    `;
}

function getAccountCard(item) {
    var codeBlock = `
    <!-- item card -->
    <div class="w-100 bc-primary rounded-3 p-3 d-flex flex-column">
        <!-- label -->
        <span class="app-brand-text demo menu-text text-white fs-4 fw-bold me-2" style="text-shadow: 0 0 1px var(--c-primary)">حساب کاربری</span>
        <hr class="text-white">

            <!-- user informations -->
            <div class="d-flex flex-column align-items-center">
            <input hidden value="${item.id}" id="txtAccountUserId" />

                <!-- user image -->
                <div class="position-relative d-inline-block my-3 userInfo-image" onclick="onEdit(1)">
                    <img id="imgAccountUserImage" src="${item.imageName}" class="rounded-circle d-block" width="225px" height="225px" style="box-shadow: 0 0 10px 5px white;" alt="user profile image">
                    <div class="userInfo-imageOverlay position-absolute top-0 left-0 w-100 h-100 d-flex justify-content-center align-items-center rounded-circle opacity-0 cursor-pointer" style="width: 115px; height: 115px;">
                        <input type="file" name="txtAccountUserImage" id="txtAccountUserImage" hidden>
                        <i class="bi bi-pencil-square text-white hc-fs-paragraph1"></i>
                    </div>
                </div>

                <!-- user fullname -->
                <h3 class="hc-fs-paragraph1 text-center userInfo-lblFullname p-2 rounded-3 cursor-pointer" id="userInfo-lblFullname" onclick="toggleSections('userInfo-lblFullname', 'userInfo-txtFullname')">${item.fullName}</h3>
                <div id="userInfo-txtFullname" class="d-none justify-content-center align-items-center my-2 p-2 rounded-3 bc-darkBlue">
                    <input type="text" class="hc-fs-paragraph1 border-0 text-white bg-transparent" value="${item.fullName}" name="FullName" id="txtAccountUserFullName">
                </div>

                <!-- user username -->
                <h5 class="hc-fs-paragraph2 cursor-pointer mb-3 d-flex align-items-center" dir="ltr">
                @
                <span class="tc-lightBlue text-decoration-underline" onclick="SaveToClipboard(this)" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="کپی نام کاربری">${item.username}
                </span>
                </h5>

                <!-- user rank && user status -->
                <div class="row d-flex justify-content-center align-items-center w-100 mb-3">
                    <div class="col">
                        <h3 class="w-100 hc-fs-paragraph1 text-center userInfo-lblRank p-2 rounded-3 cursor-pointer bc-secondary" id="userInfo-lblRank" onclick="toggleSections('userInfo-lblRank', 'userInfo-txtRank')">${item.status}</h3>
                        <div id="userInfo-txtRank" class="d-none justify-content-center align-items-center my-2 p-2 rounded-3 bc-darkBlue">
                            <input type="text" class="hc-fs-paragraph1 border-0 text-white bg-transparent" value="${item.rank}" name="Rank" id="txtAccountUserRank">
                        </div>
                    </div>
                    <div class="col">
                        <h3 class="w-100 hc-fs-paragraph1 text-center userInfo-lblStatus p-2 rounded-3 cursor-pointer bc-secondary" id="userInfo-lblStatus" onclick="toggleSections('userInfo-lblStatus', 'userInfo-txtStatus')">${item.rank}</h3>
                        <div id="userInfo-txtStatus" class="d-none justify-content-center align-items-center my-2 p-2 rounded-3 bc-darkBlue">
                            <input type="text" class="hc-fs-paragraph1 border-0 text-white bg-transparent" value="${item.status}" name="Status" id="txtAccountUserStatus">
                        </div>
                    </div>
                </div>

                <!-- user email && user pass -->
                <div class="row d-flex justify-content-center align-items-center w-100 mb-3">
                    <div class="col">
                        <h3 class="w-100 hc-fs-paragraph1 text-center userInfo-lblEmail p-2 rounded-3 cursor-pointer bc-secondary" id="userInfo-lblEmail" onclick="toggleSections('userInfo-lblEmail', 'userInfo-txtEmail')" hidden>${item.email}</h3>
                        <div id="userInfo-txtEmail" class="d-none justify-content-center align-items-center my-2 p-2 rounded-3 bc-darkBlue">
                            <input type="email" class="hc-fs-paragraph1 border-0 text-white bg-transparent" value="${item.email}" name="Email" id="txtAccountUserEmail">
                        </div>
                    </div>
                    <div class="col">
                        <input type="password" class="w-100 hc-fs-paragraph1 text-center userInfo-lblPass p-2 rounded-3 cursor-pointer bc-secondary border border-0 text-white hc-fs-paragraph2 mb-2" id="userInfo-lblPass" onclick="toggleSections('userInfo-lblPass', 'userInfo-txtPass')" hidden value="${item.password}">
                        <div id="userInfo-txtPass" class="d-none justify-content-center align-items-center my-2 p-2 rounded-3 bc-darkBlue">
                            <input type="password" class="hc-fs-paragraph1 border-0 text-white bg-transparent" value="${item.password}" name="Password" id="txtAccountUserPassword">
                        </div>
                    </div>
                </div>

                <!-- submit button -->
                <div class="row w-100">
                    <button type="submit" class="col btn btn-outline-warning" onclick="EditUser()">ویرایش</button>
                </div>

            </div>
    </div>
    <!-- end item card -->
    `;
    return codeBlock;
}
function getRelationCard(item) {
    var codeBlock = `
                <!-- item card -->
                <div class="col-lg-4 col-md-4 col-sm-6 p-3">
                <a onclick="ShowProjectsByOrgId(${item.organization.id})" class="w-100">
                  <div class="userPanel-itemCard hc-box bc-secondary mt-2">
                      <div class="d-flex flex-column justify-content-center align-items-center">
                        <!-- item card image -->
                        <img src="${item.organization.imageName}" alt="item card image" width="100px" height="100px" class="bg-white rounded-circle p-1">
                        <!-- item card title -->
                        <h3 class="hc-fs-title3 my-2">${item.organization.title}</h3>
                        <!-- item card paragraph -->
                        <h3 class="hc-fs-paragraph3 my-2">${item.organization.owner.fullName}</h3>
                      </div>
                       
                  </div>
                </a>
                </div>
                <!-- end item card -->
                    `;
    return codeBlock;
}
function getRequestCard(item) {
    var codeBlock = `
                <!-- item card -->
                <div class="col-lg-4 col-md-4 col-sm-6 p-3">
              <div class="userPanel-itemCard hc-box bc-secondary">
                <a href="#">
                  </a><div class="d-flex flex-column justify-content-center align-items-center"><a href="#">
                    <!-- item card Image -->
                    <div class="d-flex flex-column justify-content-center align-items-center w-100">
                      <img src="${item.imageName}" width="100" height="100" alt="" class="rounded-circle p-1 bg-white">

                        <!-- item card FullName -->
                        <h3 class="hc-fs-paragraph1 my-2">${item.title}</h3>

                        <!-- item card UserName -->
                        <h3 class="hc-fs-span1 my-2" dir="ltr">@ ${item.ownerName}</h3>

                    </div>

                    <!-- item card tags -->
                    </a><div class="itemCard-tagBox d-flex justify-content-center align-items-center py-3">
                    <button class="btn btn-success mx-1 text-nowrap" onclick="SetInviteAnswer(${item.id}, ${true})">
                        پذیرفتن
                      </button>
                      <button class="btn btn-danger mx-1 text-nowrap" onclick="SetInviteAnswer(${item.id}, ${false})">
                        رد کردن
                      </button>
                    </div>
                  </div>

              </div>
            </div>
                <!-- end item card -->
                    `;
    return codeBlock;
}
function getReportsCard(item) {
    var codeBlock = `
            <!-- item card -->
            <h1 class="hc-fs-title1">
                به زودی ...
            </h1>
            <!-- end item card -->
                    `;
    return codeBlock;
}
function getProjectCard(item) {
    var codeBlock = `
            <!-- item card -->
                        <div class="col-lg-4 col-md-4 col-sm-6 p-3">
                            <div class="userPanel-itemCard hc-box bc-darkBlue">
                                    <div class="d-flex flex-column justify-content-center align-items-center">
                                        <!-- item card image -->
                                        <img class="bg-white rounded-circle p-3" src="/src/media/Logo/regular-favicon-color.png" alt="item card image" width="85px"
                                                height="85px" />
                                        <!-- item card title -->
                                        <h3 class="hc-fs-paragraph2 my-2 text-center truncate" onclick="ShowTasksByProjectId(${item.id})">
                                            <span class="itemCard-title truncate">${item.title}</span>
                                        </h3>
                                        <!-- item card tags -->
                                        <div class="itemCard-tagBox d-flex flex-column justify-content-center align-items-center py-3">
                                            <span class="itemCard-tag p-1 px-3 bc-secondary rounded-3 text-white hc-fs-span3 my-1">
                                                انجام شده : ${item.tasksStatusPercent}%
                                            </span>
                                            <span class="itemCard-tag p-1 px-3 bc-secondary rounded-3 text-white hc-fs-span3 my-1">
                                                تعداد وظایف : ${item.tasksCount}
                                            </span>
                                        </div>
                                        <!-- item card buttons -->
                                        <div class="row w-100">
                                        <!--
                                            <div class="col-sm-12 col-md-6 px-1">
                                            <button onclick="EditItem('پروژه', ${item.id})" class="btn bg-warning w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
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
                                            -->
                                            <div class="col-12 w-100">
                                             <button onclick="ShowTasksByProjectId(${item.id})" type="button" class=" bc-primary btn btn-sm col-12 w-100 text-white d-flex justify-content-center align-items-center">
                                                 <i class="bi bi-info-square ms-1 text-white"></i>
                                                 اطلاعات بیشتر
                                             </button>
                                         </div>
                                        </div>
                                        
                                    </div>
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
                            <div class="userPanel-itemCard hc-box bc-darkBlue">
                                <div class="d-flex flex-column justify-content-center align-items-center">
                                <!-- item card code -->
                                <div class="d-flex justify-content-between align-items-center w-100">
                                    <h3 class="hc-fs-paragraph1 my-2">${item.id}</h3>
                                    <i class="bi ${priority} text-white"></i>
                                </div>

                                <!-- item card title -->
                                <h3 class="hc-fs-paragraph2 my-2 text-center truncate" onclick="ShowTaskDetails(${item.id})">
                                    <span class="itemCard-title truncate">${item.title}</span>
                                </h3>
                                <!-- item card tags -->
                                <div class="itemCard-tagBox d-flex flex-column justify-content-center align-items-center py-3">
                                    <span class="itemCard-tag p-1 px-3 bc-primary tc-lightBlue rounded-3 hc-fs-span3 my-1">
                                    پروژه : 
                                    ${item.project.title}
                                    </span>
                                    <span class="itemCard-tag p-1 px-3 bc-primary text-warning rounded-3 hc-fs-span3 my-1">
                                    ${item.taskStatus}
                                    </span>
                                    <span class="itemCard-tag p-1 px-3 bc-primary rounded-3 text-white hc-fs-span3 my-1">
                                    ${item.taskType}
                                    </span>
                                </div>

                                <!-- item card buttons -->
                                <div class="row w-100">
                                <!-- 
                                  <div class="col-sm-12 col-md-6 px-1">
                                    <button onclick="EditItem('وظیفه', ${item.id})" class="btn bg-warning w-100 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
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
                                  -->
                                    <div class="col-12 w-100">
                                        <button" onclick="ShowTaskDetails(${item.id})" type="button" class=" bc-primary btn btn-sm col-12 w-100 text-white d-flex justify-content-center align-items-center">
                                            <i class="bi bi-info-square ms-1 text-white"></i>
                                            اطلاعات بیشتر
                                        </button>
                                    </div>
                                </div>
                                

                                </div>
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
                <button onclick="ShowTasksByProjectId(${item.projectId})" class="btn btn-outline-primary col-12 col-sm-6 col-md-4 col-xl-3 d-flex justify-content-center align-items-center hc-fs-span3 my-1">
                  <i class="bi bi-arrow-left-square-fill tc-lightCyan hc-fs-paragraph1 ms-2"></i>
                  <span class="hc-fs-paragraph2">بازگشت</span>
                </button>
              <div class="taskDetailBox bc-primary w-100 rounded-3 d-flex flex-column p-3" style="box-shadow: 0 0 10px var(--c-primary);">
              <!-- Task Control Buttons -->
              <div class="row w-100 mb-3">
                <div class="dropdown-center d-flex justify-content-end w-100 col-8 col-sm-6 col-md-4 col-xl-3">
                  <button class="btn bc-darkBlue px-3 text-white dropdown-toggle hc-fs-paragraph2" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                    وضعیت : ${item.taskStatus}
                  </button>
                  <ul id="ddlTaskStatus" class="dropdown-menu bc-darkBlue">
                    <li><a onClick="ChangeTaskStatus(this)" class="dropdown-item text-end" href="#">انجام شده</a></li>
                    <li><a onClick="ChangeTaskStatus(this)" class="dropdown-item text-end" href="#">درحال انجام</a></li>
                    <li><a onClick="ChangeTaskStatus(this)" class="dropdown-item text-end" href="#">درحال بررسی</a></li>
                  </ul>
                </div>
              </div>
                <div class="w-100 d-flex justify-content-between row">
                  <h5 class="hc-fs-paragraph3 col">کد : ${item.id}</h5>
                  <h5 class="hc-fs-paragraph3 col">پروژه : ${item.project.title}</h5>
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



// end module cards

// processes
function EditUser() {
    var userId = $("#txtAccountUserId").val();
    var userImage = accountUserImage;
    var userFullName = $("#txtAccountUserFullName").val();
    var userRank = $("#txtAccountUserRank").val();
    var userStatus = $("#txtAccountUserStatus").val();

    var formData = new FormData();
    formData.append("Id", userId);
    formData.append("FullName", userFullName);
    formData.append("Rank", userRank);
    formData.append("Status", userStatus);
    if (userImage) {
        formData.append("Image", userImage);  // Append the file if it's selected
    }

    $.ajax({
        url: "/api/Users/",
        type: "POST",
        contentType: false, // Important for file uploads
        processData: false, // Important for file uploads
        data: formData,
        success: function (data) {
            if (data.isSuccess == true) {
                toastr.success(data.message);
                ShowAccountTab();
            } else {
                toastr.error(data.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error("خطا در انجام عملیات: " + xhr.responseText);
            console.error("Error: " + error);
            console.error("Status: " + status);
            console.error("Response: ", xhr.responseText);
        }
    });
}
function SetInviteAnswer(intviteId, inviteStatus) {
    var inviteData = {
        Id: intviteId,
        IsAccepted: inviteStatus
    };

    $.ajax({
        url: "/api/Invites/SetInviteStatus",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(inviteData),
        success: function (data) {
            if (data.isSuccess == true) { toastr.success(data.message); ShowRequestTab(); }
            else { toastr.error(data.message); }
        },
        error: function (xhr) {
            toastr.error(xhr.responseTextss);
        }
    });
}
function filterItems(parameter) {
    if (document.getElementsByClassName("userPanel-menuItem")[1].classList.contains("userPanel-menuItem-active"))  // projects tab selected
        UpdateRelationsByFilter(parameter);
    else if (document.getElementsByClassName("userPanel-menuItem")[2].classList.contains("userPanel-menuItem-active")) // tasks tab selected
        UpdateRequestsByFilter(parameter);
}
//end processes