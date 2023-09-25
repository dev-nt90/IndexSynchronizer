// initialize the status icon to the default (neutral)
const statusIndicator = document.querySelector('.status-indicator');
const successIcon = statusIndicator.querySelector('.success-icon');
const failureIcon = statusIndicator.querySelector('.failure-icon');
const neutralIcon = statusIndicator.querySelector('.neutral-icon');

successIcon.style.display = 'none';
failureIcon.style.display = 'none';
neutralIcon.style.display = 'inline';

function updateStatusIndicator(isSuccess) {
    const statusIndicator = document.querySelector('.status-indicator');
    const successIcon = statusIndicator.querySelector('.success-icon');
    const failureIcon = statusIndicator.querySelector('.failure-icon');

    // on first success/failure, disable the neutral icon - this work is duplicated on additional passes, but for now I don't care
    const neutralIcon = statusIndicator.querySelector('.neutral-icon');
    neutralIcon.style.display = 'none';

    if (isSuccess) {
        successIcon.style.display = 'inline'; // Show the success icon
        failureIcon.style.display = 'none'; // Hide the failure icon
    } else {
        successIcon.style.display = 'none'; // Hide the success icon
        failureIcon.style.display = 'inline'; // Show the failure icon
    }
}