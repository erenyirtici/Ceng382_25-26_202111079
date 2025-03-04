document.addEventListener("DOMContentLoaded", function() {
    let video = document.getElementById("bg-video");
    let playButton = document.getElementById("play-login");
    let mushroomIcon = document.getElementById("mushroom-icon");
    let coinIcon = document.getElementById("coin-icon");
    let marioVideoContainer = document.getElementById("mario-video-container");
    let marioVideo = document.getElementById("mario-video");
    let closeMarioVideo = document.getElementById("close-mario-video");

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
            if (media !== marioVideo) { // Mario videosu hariç
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
});
