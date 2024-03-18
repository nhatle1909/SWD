const redirectUrl = new URL(window.location.href); // Use full redirect URL
 // Use);.origin); // Use
const popup = document.getElementById('popup');
const message = document.getElementById('message');
const icon = document.querySelector('#popup .icon');
const closeButton = document.getElementById('close-button');

function redirectToHome()
{
window.location.href = 'http://localhost:3000';
}
closeButton.addEventListener('click',redirectToHome);

function showPopup(text, status) {
  message.textContent = text;
  icon.classList.remove('success', 'error');
  icon.classList.add(status);
  popup.classList.remove('hidden', 'visible');
  popup.classList.add('visible'); // Show the popup with an animation
}

fetch('https://swdapi.azurewebsites.net/api/Transaction/VNPay-Return?url=' +encodeURIComponent(redirectUrl.href)) // Send the entire redirect URL
  .then(response => {
    if (response.ok) {
      showPopup('Checkout Successful!', 'success');
      
    } else {
      showPopup('Checkout Failed!', 'error');
    }
  })
  .catch(error => {
    console.error('Error fetching API:', error);
    showPopup('An error occurred. Please try again.', 'error');
  });

closeButton.addEventListener('click', () => {
  popup.classList.remove('visible');
});

setTimeout(() => {
 window.location.href('http://localhost:3000'); // Hide the popup after a delay
}, 5000); // Adjust the delay as needed