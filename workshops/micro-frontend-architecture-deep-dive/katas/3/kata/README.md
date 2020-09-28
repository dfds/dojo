DFDS MFA Training - Code Kata #3
======================================

This training exercise is a **beginner-level** course on micro frontend architecture that serves as a starting point for developers looking to onboard the MFA efforts at DFDS.


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the correct tools installed and configured.


### Prerequisites

* [Visual Studio Code](https://code.visualstudio.com/download)


## Exercise

Your third assignment will see you create your very own custom element which can be imported and used in any HTML5-enabled context!


### 1. Create your project directory
`mkdir /kata3`<br/>
`cd /kata3`


### 2. Create a simple HTML document
Create a file named `fun-with-custom-elements.html` containing the following code:

```
<html>
<body>
</body>
</html>
```

***Note*** <br/>
You can use `vi` to edit the file. `vi fun-with-custom-elements.html` will create the file and open the editor.


### 2. Create a javascript file for our custom-element code
Create a file named `custom-element.js` and add the following code:

```
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
```


### 3. Add reference to our newly created javascript file and register our custom-element
Open `fun-with-custom-elements.html` to add our script reference and register our custom element with the customElements API define method:

```
<html>
<body>  
    <script src="custom-element.js"></script>
    <script>      
        customElements.define('custom-element', CustomElement); 
    </script>
</body>
</html>
```


### 4. We can easily extend our existing component and create new child components
Import a template from https://raw.githubusercontent.com/dfds/ded-dojo/master/workshops/micro-frontend-architecture-deep-dive/katas/2/final/messages.html via a link element and add a script block to host and register our new child component. 

```
<html>
<body>  
    <script src="custom-element.js"></script>
    <script>      
        customElements.define('custom-element', CustomElement); 
    </script>    
    <link rel="import" href="https://raw.githubusercontent.com/dfds/ded-dojo/master/workshops/micro-frontend-architecture-deep-dive/katas/2/final/messages.html" />
    <script>    
        class CustomElementChild extends CustomElement
        {
            attachedCallback() {
                const htmlImport = document.querySelector('link[rel="import"]').import;
                const htmlTemplate = htmlImport.querySelector('#mytemplate');

                this.appendChild(document.importNode(htmlTemplate.content, true));   
            };
        }

        customElements.define('custom-element-child', CustomElementChild);
    </script>
</body>
</html>
```


### 5. Add our newly minted custom elements to the page
Once we have defined our custom elements in the DOM using them is simply a matter adding our custom mark-up.

```
<html>
<body>  
    <script src="custom-element.js"></script>
    <script>      
        customElements.define('custom-element', CustomElement); 
    </script>    
    <link rel="import" href="https://raw.githubusercontent.com/dfds/ded-dojo/master/workshops/micro-frontend-architecture-deep-dive/katas/2/final/messages.html" />
    <script>    
        class CustomElementChild extends CustomElement
        {
            attachedCallback() {
                const htmlImport = document.querySelector('link[rel="import"]').import;
                const htmlTemplate = htmlImport.querySelector('#mytemplate');

                this.appendChild(document.importNode(htmlTemplate.content, true));   
            };
        }

        customElements.define('custom-element-child', CustomElementChild);
    </script>
    <custom-element/>
    <custom-element-child/>
</body>
</html>
```

***Note*** <br/>
You can use the browsers developer tooling via `F12` to via the console.log messages and verify the components are working.


## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/ded-dojo/issues).
 