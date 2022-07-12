DFDS Event API Deep Dive - Code kata #3
======================================

This training exercise is a **beginner-level** course on event APIs that serve as a starting point for Developers looking to get started with event-driven systems at DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [NodeJS](https://nodejs.org/en/download/)

## Exercise
In this exercise we will learn about `wscat` a light-weight CLI tool that can help increase our productivity as developers in DFDS when we are working with websockets-based event APIs. 


### 1. Install wscat via NPM
The wscat CLI utility can be downloaded via NPM using the following command:

```
npm install -g wscat
```

Just to explain: <br/>
`npm` - is the NodeJS Package Manager CLI <br/>
`install` - instructs the npm CLI to install the wscat package<br/>
`-g` - tells the npm CLI to target a global user scope


### 2. Ensure that wscat is working
Once we have wscat installed we can use the `--help` to verify that its working as intended: 

```
wscat --help
```


### 3. Try it out
Lastly we can test the tool with a public echo service or the one we built in [Kata #2](../../2/kata/README.md) as follows:

```
wscat -c ws://websocket-echo.com
```


## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 