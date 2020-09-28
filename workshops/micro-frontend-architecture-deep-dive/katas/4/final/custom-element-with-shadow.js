class CustomElementWithShadow extends HTMLElement { 
    constructor() {
        super();

        // Create a shadow root
        let shadow = this.attachShadow({mode: 'open'});

        // Create simple span
        let infoSpan = document.createElement('span');
        infoSpan.setAttribute('class', 'info');

        // Take attribute content and put it inside the info span
        infoSpan.textContent = this.getAttribute('data-text');

        // Create some CSS to apply to the shadow dom
        let style = document.createElement('style');

        style.textContent = `
        .info {
            font-size: 1.8rem;
            position: relative;
        }`;

        // attach the created elements to the shadow dom
        shadow.appendChild(style);
        shadow.appendChild(infoSpan);
    }
}

customElements.define('custom-element-with-shadow', CustomElementWithShadow);