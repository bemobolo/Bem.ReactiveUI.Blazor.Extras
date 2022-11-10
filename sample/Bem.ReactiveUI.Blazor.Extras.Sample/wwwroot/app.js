$(function() {
// Select the node that will be observed for mutations
const targetNode = document.getElementsByTagName("body")[0];

// Options for the observer (which mutations to observe)
const config = { attributes: true, childList: true, subtree: true, characterData: true };

// Callback function to execute when mutations are observed
const callback = (mutationList, observer) => {
  for (const mutation of mutationList) {
      if (mutation.type === "childList") {
          console.log("A child node has been added or removed.");
      } else if (mutation.type === "attributes") {
          console.log(`The ${mutation.attributeName} attribute was modified.`);
      } else if (mutation.type == "characterData") {
          const map = Map.groupBy(mutationList.map(mutationRecord => {
              var target = mutationRecord.target;
              while (target && (!target.classList || !target.classList.contains("flash"))) {
                  target = target.parentElement;
              }
              return target && target.classList ? target : null;
          })
          .filter(target => target), target => target);
          const iter = Array.from(map.values());
          const targets = iter.map(targets => targets[0]);
          
          var timeout = 0;
          const delay = 300;
          for (const target of targets) {
            window.setTimeout(() => {
                target.classList.remove("flash");
                void target.offsetWidth;
                target.classList.add("flash");
            }, timeout);
            timeout += delay
          }
          console.log("CharacterData changes");
      }
  }
};

// Create an observer instance linked to the callback function
const observer = new MutationObserver(callback);

// Start observing the target node for configured mutations
observer.observe(targetNode, config);

// Later, you can stop observing
// observer.disconnect();

});