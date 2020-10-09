DFDS MFA Training - Code Kata #1
======================================

This training exercise is a **beginner-level** course on micro frontend architecture that serves as a starting point for developers looking to onboard the MFA efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Visual Studio Code](https://code.visualstudio.com/download)

## Exercise
Your first assignment will see you build a simple template that will allows us to declare a re-usable fragments of HTML that can be rendered as part of our DOM.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata1
cd kata1
```

### 2. Create a simple HTML document
It's pretty simple: create a file named `fun-with-templates.html` containing the following code:

```
<html>
<body>
</body>
</html>
```

***Note*** <br/>
You can use `vi` to edit the file. `vi fun-with-templates.html` will create the file and open the editor.

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
Now that we have created our UI fragment we need to add a script block to our document in order to fetch the template element via the browsers querySelector API which will yield a result that expose the "template DOM" via a `content` property that we can manipulate to load and configure elements nested within the template. E.g. set the source of our image to `does-not-exist.png` and the comment to `lorum ipsum`. 

Once the template node has been data-bound (populated) we can proceed to using the importNode API to inject (clone) a copy of it into the "document DOM".

```
<html>
<body>
    <template id="mytemplate">
        <img src="" alt="great image">
        <div class="comment"></div>
    </template>
    <script>
        var t = document.querySelector('#mytemplate');
        
        t.content.querySelector('img').src = 'logo.png';
        t.content.querySelector('div').innerHTML = 'lorum ipsum';

        var clone = document.importNode(t.content, true);

        document.body.appendChild(clone);
    </script>
</body>
</html>
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
