class CustomElement extends HTMLElement {
    // Fires when an instance of the element is created.
    createdCallback() {
        console.log('Created', this);
    };
    // Fires when an instance was inserted into the document.
    attachedCallback() {
        console.log('Attached', this);
    };
    // Fires when an instance was removed from the document.
    detachedCallback() {
        console.log('Detatched', this);
    };
    // Fires when an attribute was added, removed, or updated.
    attributeChangedCallback(attr, oldVal, newVal) {
        console.log('Attribute changed', attr, oldVal, newVal);
    };
}