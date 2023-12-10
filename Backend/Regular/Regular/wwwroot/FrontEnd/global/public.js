// Loading Page
function ShowLoadingPageAnimation(duration = 3000) {
    document.getElementsByTagName("body")[0].style = "overflow-y: hidden";
    //this function show loading animation in each page
    let startTime = new Date();
    window.addEventListener("DOMContentLoaded", event => {
        let EndTime = new Date();
        if (EndTime - startTime > duration) {
            $("#loader-animation").remove();
            document.getElementsByTagName("body")[0].style = "overflow-x: auto";
        }
        else {
            window.setTimeout(e => {
                $("#loader-animation").remove();
                document.getElementsByTagName("body")[0].style = "overflow-x: auto";
            }, duration - (EndTime - startTime));
        }
    });
}