var zIndexCounter = 0;

var mainDiv;

var mediaPlaying = {};

var videoProperties = {};

$.fn.extend({
	animateCss: function (animationName, callback) {
		var animationEnd = (function (el) {
			var animations = {
				animation: 'animationend',
				OAnimation: 'oAnimationEnd',
				MozAnimation: 'mozAnimationEnd',
				WebkitAnimation: 'webkitAnimationEnd',
			};

			for (var t in animations) {
				if (el.style[t] !== undefined) {
					return animations[t];
				}
			}
		})(document.createElement('div'));

		if (animationName) {
			this.addClass('animated ' + animationName).one(animationEnd, function () {
				$(this).removeClass('animated ' + animationName);

				if (typeof callback === 'function') callback();
			});
		}
		else if (typeof callback === 'function') callback();

		return this;
	},
});

function addImage(id, scene, link, width, height, horizontal, vertical, visible, makeButton) {
    var newElement = document.createElement('img');
    newElement.src = link;
    newElement.style.cssText += 'width: ' + width + 'px; height: ' + height + 'px; ';

    addElement(newElement, id, scene, horizontal, vertical, visible, makeButton);
}

function addText(id, scene, text, size, color, font, horizontal, vertical, visible, makeButton) {
    var newElement = document.createElement('h1');
    newElement.innerHTML = text;
    newElement.style.cssText += 'font-size: ' + size + 'px; color: ' + color + '; ' + 'text-shadow: -1px 0 black, 0 1px black, 1px 0 black, 0 -1px black; white-space: nowrap;';

    addElement(newElement, id, scene, horizontal, vertical, visible, makeButton);
}

function playSound(link) {
    if (mediaPlaying[link]) {
        return;
    }
    mediaPlaying[link] = true;

    var audio = new Audio(link);
    audio.play();

    setTimeout(function () {
        mediaPlaying[link] = false;
    }, 2000);
}

function playVideo(link) {
    if (mediaPlaying[link]) {
        return;
    }
    mediaPlaying[link] = true;

    if (videoProperties[link]) {
        var newElement = document.createElement('video');
        newElement.width = videoProperties[link].width;
        newElement.height = videoProperties[link].height;
        newElement.frameBorder = 0;
        newElement.allow = "encrypted-media";
        newElement.setAttribute('autoplay', '');

        var sourceElement = document.createElement('source');
        sourceElement.src = link;
        if (link.endsWith(".mp4")) {
            sourceElement.type = "video/mp4";
        }
        else if (link.endsWith(".webm")) {
            sourceElement.type = "video/webm";
        }
        newElement.appendChild(sourceElement);

        addElement(newElement, link, "Video", videoProperties[link].x, videoProperties[link].y, true, false);

        var promise = newElement.play();
        if (promise !== undefined) {
            promise.then(_ => {
                // Autoplay started!
            }).catch(error => {
                removeElement(newElement);
            });
        }

        newElement.onended = function () {
            removeElement(newElement);
        };

        setTimeout(function () {
            mediaPlaying[link] = false;
        }, 2000);
    }
}

function addElement(element, id, scene, horizontal, vertical, visible, makeButton) {
    if (element != null) {
        element.id = id;
        element.style.cssText += 'position: absolute; left: ' + horizontal.toString() + '%; top: ' + vertical.toString() + '%; transform: translate(-50%, -50%);'
        element.style.zIndex = zIndexCounter++;

        if (!visible) {
            element.style.cssText += 'visibility: hidden;'
        }

        if (makeButton) {
            element.addEventListener('click', function () { buttonClicked(id); }, false);
        }

        mainDiv.appendChild(element);
    }
}

function removeElement(element) {
    if (element != null) {
        mainDiv.removeChild(element);
    }
}

function buttonClicked(id) {
    mixer.socket.call('giveInput', {
        controlID: id,
        event: 'mousedown'
    });
}

function handleVideoResized(position) {
	const overlay = document.getElementById('overlay');
	const player = position.connectedPlayer;
	overlay.style.top = `${player.top}px`;
	overlay.style.left = `${player.left}px`;
	overlay.style.height = `${player.height}px`;
	overlay.style.width = `${player.width}px`;
}

function handleControlUpdate(update) {
    if (update.controls != null) {
        for (var i = 0; i < update.controls.length; i++) {
            var controlID = update.controls[i].controlID;
            var element = document.getElementById(controlID);
            if (element != null) {
                var metadata = update.controls[i].meta;

                if (metadata.visible != null) {
                    if (metadata.visible) {
                        element.style.cssText += 'visibility: visible;'
                    }
                    else {
                        element.style.cssText += 'visibility: hidden;'
                    }
                }

                if (metadata.playsound != null) {
                    playSound(metadata.playsound);
                }

                if (metadata.playvideo != null) {
                    playVideo(metadata.playvideo);
                }
            }
        }
    }
}

window.addEventListener('load', function initMixer() {
	
	mainDiv = document.getElementById('mainDiv');
	
	mixer.display.position().subscribe(handleVideoResized);

    mixer.isLoaded();

    initializeItems();
});

mixer.socket.on('onControlUpdate', handleControlUpdate);