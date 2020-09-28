DFDS MFA Training - Code Kata #1
======================================

This training exercise is a **beginner-level** course on micro frontend architecture that serves as a starting point for developers looking to onboard the MFA efforts at DFDS.


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the correct tools installed and configured.


### Prerequisites

* [Visual Studio Code](https://code.visualstudio.com/download)


## Exercise

Your first assignment will see you build a simple template that will allows us to declare a re-usable fragments of HTML that can be rendered as part of our DOM.


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


### 3. Create a simple template
Now that we have a HTML document to work in its time to add a template element to it so we can specify the layout of our re-usable UI fragment. We will add a simple element with the id `mytemplate` which contains a image element for the worlds greatest image and a comment field for ad-hoc shout-outs.

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


### 4. Use the template to inject new UI elements in our DOM 
Now that we have created our UI fragment we need to add a script block to our document in order to fetch the template element via the browsers querySelector API which will yield a result that expose the "template DOM" via a `content` property which we can manipulate to load and configure elements nested within the template. E.g. set the source of our image to `does-not-exist.png` and the comment to `lorum ipsum`. 

Once the template node has been data-bound (populated) we can proceed to using the importNode API to inject a copy of itself into the "document DOM".

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
