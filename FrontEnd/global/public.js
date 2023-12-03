// Loading Page
function ShowLoadingPageAnimation(duration = 3000) {
    //this function show loading animation in each page
    let startTime = new Date();
    window.addEventListener("DOMContentLoaded", event => {
        let EndTime = new Date();
        if (EndTime - startTime > duration) {
            $("#loader-animation").remove();
        }
        else {
            window.setTimeout(e => {
                $("#loader-animation").remove();
            }, duration - (EndTime - startTime));
        }
    });
}