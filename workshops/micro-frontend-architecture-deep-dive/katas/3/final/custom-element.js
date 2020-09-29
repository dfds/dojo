class CustomElement extends HTMLElement {
    connectedCallback() {
        console.log('Connected', this);
    };
}