DFDS MFA Training - Code Kata #2
======================================

This training exercise is a **beginner-level** course on micro frontend architecture that serves as a starting point for developers looking to onboard the MFA efforts at DFDS.


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the correct tools installed and configured.


### Prerequisites

* [Visual Studio Code](https://code.visualstudio.com/download)


## Exercise

Your fourth assignment will get you acquainted with Shadow DOM.


### 1. Create your project directory
`mkdir /kata4`<br/>
`cd /kata4`


### 2. Create a simple HTML document
It's pretty simple: create a file named `fun-with-shadow-dom.html` containing the following code:

```
<html>
<body>
</body>
</html>
```

***Note*** <br/>
You can use `vi` to edit the file. `vi fun-with-shadow-dom.html` will create the file and open the editor.


### 3. Create a javascript file for our custom-element code
Create a file named `custom-element-with-shadow.js` and add the following code:

```
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
```

### 4. Add reference to our newly created javascript file
Open `fun-with-shadow-dom.html` to add our newly created script resource:

```
<html>
<body>
    <script src="custom-element-with-shadow.js"></script>
</body>
</html>
```


### 5. Add our newly minted custom elements to the page
Simple add the `custom-element-with-shadow` tag above our script block. Use the `data-text` attribute to data-bind a custom message to our component logic which is currently nested inside our Shadow DOM. 

```
<html>
<body>
    <custom-element-with-shadow data-text="Hello World!" />
    <script src="custom-element-with-shadow.js"></script>
</body>
</html>
```

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/ded-dojo/issues).
 