DFDS MFA Training - Code Kata #2
======================================

This training exercise is a **beginner-level** course on micro frontend architecture that serves as a starting point for developers looking to onboard the MFA efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata exercise and ensure that your local machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and create a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Visual Studio Code](https://code.visualstudio.com/download)

## Exercise
Your second assignment will see you import an existing template from https://raw.githubusercontent.com/dfds/ded-dojo/master/workshops/micro-frontend-architecture-deep-dive/katas/2/final/messages.html and use it to inject some content into a new document.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata2
cd kata2
```

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

### 4. Extract the template from our imported document
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
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 