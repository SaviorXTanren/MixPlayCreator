var zIndexCounter = 0;

var mainDiv;

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

function addElement(newElement, id, scene, horizontal, vertical, visible, makeButton) {
    if (newElement != null) {
        newElement.id = id;
        newElement.style.cssText += 'position: absolute; left: ' + horizontal.toString() + '%; top: ' + vertical.toString() + '%; transform: translate(-50%, -50%);'
        newElement.style.zIndex = zIndexCounter++;

        if (!visible) {
            newElement.style.cssText += 'visibility: hidden;'
        }

        if (makeButton) {
            newElement.addEventListener('click', function () { buttonClicked(id); }, false);
        }

        mainDiv.appendChild(newElement);
    }
}

function buttonClicked(id) {
    mixer.socket.call('giveInput', {
        controlID: id,
        event: 'mousedown'
    });
}

function playSound(link) {
    setTimeout(function () {
        var audio = new Audio(link);
        audio.play();
    }, 100);
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

}

window.addEventListener('load', function initMixer() {
	
	mainDiv = document.getElementById('mainDiv');
	
	mixer.display.position().subscribe(handleVideoResized);

    mixer.isLoaded();

    initializeItems();
});

mixer.socket.on('onControlUpdate', handleControlUpdate);