document.addEventListener("DOMContentLoaded", function() {
    let video = document.getElementById("bg-video");
    let playButton = document.getElementById("play-login");
    let mushroomIcon = document.getElementById("mushroom-icon");
    let coinIcon = document.getElementById("coin-icon");
    let marioVideoContainer = document.getElementById("mario-video-container");
    let marioVideo = document.getElementById("mario-video");
    let closeMarioVideo = document.getElementById("close-mario-video");
    let clock = document.getElementById("clock");

    let marioBlocks = document.querySelectorAll(".mario-block, .mario-block-right");

    let loginForm = document.getElementById("login-form"); 
    let userList = []; //array for users
    // AI Prompt : I want the background video to start automatically when the page loads
    video.currentTime = 0;
    video.play().catch(error => console.log("Autoplay engellendi:", error));

    // AI Prompt : When the video is completed, go back to the beginning
    video.addEventListener("ended", function() {
        video.currentTime = 0;
    });

    
    playButton.addEventListener("click", function() {
        video.currentTime = 0;
        video.play();
    });

    // AI Prompt : How do I mute all videos and audio on my site
    function muteAllSounds() {
        document.querySelectorAll("video, audio").forEach(media => {
            if (media !== marioVideo) { // Except for the Mario video.
                media.muted = true;
            }
        });
    }

    
    function unmuteAllSounds() {
        document.querySelectorAll("video, audio").forEach(media => {
            media.muted = false;
        });
    }

    
    function playMarioVideo() {
        muteAllSounds(); 
        marioVideoContainer.classList.add("active"); 
        marioVideoContainer.style.display = "flex"; 
        marioVideo.currentTime = 0; 
        marioVideo.play();
    }

    
    function closeMarioVideoFunc() {
        marioVideo.pause();
        marioVideo.currentTime = 0;
        marioVideoContainer.style.display = "none"; 
        marioVideoContainer.classList.remove("active");
        unmuteAllSounds(); 
    }

    mushroomIcon.addEventListener("click", playMarioVideo);
    coinIcon.addEventListener("click", playMarioVideo);
    closeMarioVideo.addEventListener("click", closeMarioVideoFunc);

    // AI prompt: Create a real-time clock function using JavaScript. Get the hours, minutes, and seconds and update them every second."
    function updateClock() {
        //AI Prompt : Use JavaScript's Date object to get the current time.
        let now = new Date();
        let hours = now.getHours().toString().padStart(2, '0');
        let minutes = now.getMinutes().toString().padStart(2, '0');
        let seconds = now.getSeconds().toString().padStart(2, '0');
        clock.innerText = `${hours}:${minutes}:${seconds}`;
    }

    setInterval(updateClock, 1000); 
    updateClock(); 
    
    //AI Prompt : Create a login form. Save username and password in an array. Print the entire user list to the console after the user logs in.
    loginForm.addEventListener("submit", function(event) {
        event.preventDefault(); //Prevent page from refreshing

        let username = document.getElementById("username").value; 
        let password = document.getElementById("password").value; 

        let userData = { username: username, password: password }; 
        userList.push(userData); 

        console.log("All users:", userList); 
        loginForm.reset();
    });
    //AI Prompt : "Control the click event with JavaScript for multiple Mario blocks and generate a coin on each click."
    marioBlocks.forEach(marioBlock => {
        marioBlock.addEventListener("click", function () {
            let coin = document.createElement("div");
            coin.classList.add("coin");
            document.body.appendChild(coin);

            //AI Prompt : What method can I use in java script to play a sound effect?
            let coinSound = new Audio("coin-sound.mp3");
            coinSound.volume = 0.5; 
            coinSound.play(); 

            // Get the location of the clicked block
            let blockRect = marioBlock.getBoundingClientRect();

            
            coin.style.left = blockRect.left + 15 + "px"; 
            coin.style.bottom = window.innerHeight - blockRect.top + "px"; 

            // Start Launch Animation
            setTimeout(() => {
                coin.classList.add("jump");
            }, 10);

            // Delete Gold After Animation Ends
            setTimeout(() => {
                coin.remove();
            }, 500);
        });
    });

});
//AI Prompt : Write JavaScript code that hides all form elements when the user presses the 'H' key and brings them back when pressed again.
document.addEventListener("keydown", function(event) {
    if (event.key.toLowerCase() === "h") { 
        let formElements = document.querySelectorAll("form, input, label, button"); 
        //AI Prompt : Hide or show by changing the visibility of each form element.
        formElements.forEach(element => {
            element.style.display = (element.style.display === "none") ? "block" : "none";
        });
    }
});


