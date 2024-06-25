

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

  function onEdit(inputIndex){
    switch(inputIndex){
      case 1: // edit image
      let input = document.createElement('input');
      input.type = 'file';
      input.click();
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
    }
  }

  document.addEventListener('click', function(event) {
    if (!sectionTwo.contains(event.target) && !sectionOne.contains(event.target)) {
      sectionOne.classList.remove('d-none');
      sectionTwo.classList.add('d-none');
    } 
 });



