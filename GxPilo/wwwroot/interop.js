// wwwroot/interop.js
window.callAureliaGlobal = (src) => {
    console.log(`Calling Aurelia function with src: ${src}`);
    if (typeof window.myAppEntry === 'function') {
        window.myAppEntry(src); // Call the Aurelia function
    } else {
        console.error('window.myAppEntry is not a function');
    }
};