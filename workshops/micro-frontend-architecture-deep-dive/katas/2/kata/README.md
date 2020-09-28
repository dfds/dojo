DFDS MFA Training - Code Kata #2
======================================

This training exercise is a **beginner-level** course on micro frontend architecture that serves as a starting point for developers looking to onboard the MFA efforts at DFDS.


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the correct tools installed and configured.


### Prerequisites

* [Visual Studio Code](https://code.visualstudio.com/download)


## Exercise

Your second assignment will see you import an existing template from https://raw.githubusercontent.com/dfds/ded-dojo/master/workshops/micro-frontend-architecture-deep-dive/katas/2/final/messages.html and use it to inject some content into a new document.


### 1. Create your project directory
`mkdir /kata2`<br/>
`cd /kata2`


### 2. Create a simple HTML document
It's pretty simple: create a file named `fun-with-imports.html` containing the following code:

```
<html>
<body>
</body>
</html>
```

***Note*** <br/>
You can use `vi` to edit the file. `vi fun-with-imports.html` will create the file and open the editor.


### 3. Create a simple HTML import to fetch our template
Create a link element with its `rel` attribute set to `import` and a href pointing to our github-based template @ https://raw.githubusercontent.com/dfds/ded-dojo/master/workshops/micro-frontend-architecture-deep-dive/katas/2/final/messages.html. 

```
<html>
<body>
    <link rel="import" href="https://raw.githubusercontent.com/dfds/ded-dojo/master/workshops/micro-frontend-architecture-deep-dive/katas/2/final/messages.html" />
</body>
</html>
```


### 4. Fetch a template from the imported document by importing it into our import referrers and clone its into the master document
Add a script block to our import referrer which uses the querySelector API to fetch our imported document, exposed by the `import property`, which in turn can be used to retrieve a message template and append it to the master document DOM.

```
<html>
<body>
    <link rel="import" href="https://raw.githubusercontent.com/dfds/ded-dojo/master/workshops/micro-frontend-architecture-deep-dive/katas/2/final/messages.html" />
    <script>      
        var htmlImport = document.querySelector('link[rel="import"]').import;
        var htmlTemplate = htmlImport.querySelector('#mytemplate');

        document.body.appendChild(document.importNode(htmlTemplate.content, true));
    </script>
</body>
</html>
```

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/ded-dojo/issues).
 