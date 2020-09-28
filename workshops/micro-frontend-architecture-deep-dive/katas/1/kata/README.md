DFDS MFA Training - Code Kata #1
======================================

This training exercise is a **beginner-level** course on micro frontend architecture that serves as a starting point for developers looking to onboard the MFA efforts at DFDS.


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the correct tools installed and configured.


### Prerequisites

* [NodeJS](https://nodejs.org/en/)


## Exercise

Your first assignment will see you build a simple HTML Template that will allows us to declare fragments of HTML that can be cloned and inserted in a document by scripts.


### 1. Create your project directory
`mkdir /kata1`<br/>
`cd /kata1`


### 2. Create a simple HTML document
It's pretty simple: create a file named `fun-with-templates.html` containing the following code:

***Note*** <br/>
You can use `vi` to edit the file: <br/>
`fun-with-templates.html` will create the file and open the editor.

```
<html>
<body>
</body>
</html>
```


### 3. Create a simple HTML template
Now that we have a HTML document to work in its time to add a template element to it so we can specify the layout of our re-usable UI fragment. We will add a simple element with the id `mytemplate` which contains a image element for the worlds greatest image and a comment field for ad-hoc shout outs.

```
<html>
<body>
    <template id="mytemplate">
        <img src="" alt="great image">
        <div class="comment"></div>
    </template>
</body>
</html>
```


### 4. Use the HTML template to create new UI elements 
Add a script element to our document with the id `myscript`. Use the document querySelector API to fetch the previously added template element and drill into its content section to set the source of our image to `logo.png` (or any other image you might have handy if you dont want a broken image link) and the comment to `lorum ipsum`. Once the template node has been populated we can now use the importNode API to clone & bind it to the DOM.

```
<html>
<body>
    <template id="mytemplate">
        <img src="" alt="great image">
        <div class="comment"></div>
    </template>
    <script id="myscript">
        var t = document.querySelector('#mytemplate');
        
        t.content.querySelector('img').src = 'logo.png';
        t.content.querySelector('div').text = 'lorum ipsum';

        var clone = document.importNode(t.content, true);

        document.body.appendChild(clone);
    </script>
</body>
</html>
```

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/ded-dojo/issues).
 