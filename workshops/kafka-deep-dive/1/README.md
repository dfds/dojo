## Docker (optional if already installed)
* Install Docker.
  * Windows: Follow the Playbook located [here](https://wiki.dfds.cloud/en/playbooks/install/docker).
  * macOS: Follow the doc [here](https://docs.docker.com/docker-for-mac/install/)
  * Linux: Use your preferred package manager to install *docker*/*docker-engine* and *docker-compose*. If you're not on a supported platform, follow [this](https://docs.docker.com/install/linux/docker-ce/binaries/).

* Check that Docker is running and is working by using the following command in your preferred terminal emulator ```docker ps```
  
  If it works, the output should look a bit like this
  ![docker ps screenshot](img/01.png)

  If it doesn't, ask for help.

* Same deal with *docker-compose*, run `docker-compose` in a terminal emulator to make sure that it is installed and picked up by your PATH.


## Kafka test run
Before we begin using Kafka in our code for Events, let's make sure it works as expected on our local machines.

Open up a terminal emulator. Clone this Git repository to a place on your machine where you can find it, wherever works for you, e.g.

**Linux/macOS**

`mkdir -p Projects/workshop`

`cd Projects/workshop`


**Windows (using Powershell)**

`mkdir Projects/workshop`

`cd Projects/workshop`


If you have SSH keys set up and attached to your(if you have one) GitHub account, do the following

`git clone git@github.com:dfds/dojo.git`

If not, run this instead.

`git clone https://github.com/dfds/dojo.git` 

Navigate to kafka-deep-dive/1

`cd ded-dojo/workshops/kafka-deep-dive/1 `

Start up Kafka

`docker-compose up -d`

If it worked out as expected, it should look a bit like this:
![docker-compose up](img/02.png)

Now we will make use of a tool called *kafkacat*. Use of this tool natively on Windows.. is not the easiest, so to make this more simple, we'll be making use of a Docker image with the tool precompiled and installed. Go ahead and pull the following Docker image *edenhill/kafkacat:1.6.0*

`docker pull edenhill/kafkacat:1.6.0`

Usually with Kafka, one must create a Topic manually, however with how Kafka is set up in our *docker-compose.yml*, it'll automatically create any Topic names we use.

Let's try and **consume** a Topic.

`docker run -it --rm --network=development edenhill/kafkacat:1.6.0 -C -b kafka:9092 -t build.workshop.something`

Our topic name is in this case, 'build.workshop.something'.

![kafka consume 01](img/03.png)

Oh, it doesn't work? Good, this is what is supposed to happen. When we try to consume the Topic ''build.workshop.something'', it doesn't exist at the time. Now however, our Kafka instance should've created that Topic automatically since we attempted to Consume from it. Let's try again.

`docker run -it --rm --network=development edenhill/kafkacat:1.6.0 -C -b kafka:9092 -t build.workshop.something`

![kafka consume 02](img/04.png)

Now it'll continually listen for anything posted(or rather, in Kafka terms, **Produced**) to the Topic 'build.workshop.something'.

While having that running, open a separate terminal emulator. Let us try and Produce something to the Topic.

`docker run -it --rm --network=development edenhill/kafkacat:1.6.0 -P -b kafka:9092 -t build.workshop.something`

Now, the terminal should take any input you type, and as soon as you hit ENTER(or whatever creates a newline in your terminal emulator), send that content to your Topic.

For example, typing 'this sorta works', hitting ENTER, then type 'yay', then hit ENTER again, should produce the following output in your first terminal window.

![kafka consume 03](img/06.png)

Execute the following to stop the Kafka server:
`docker-compose down`

---
Great! We now have a local Kafka setup that can **Consume** from a **Topic**, as well as **Produce** to a **Topic**. Let's move on to the 2nd kata, where we'll try and do the same, but in code instead.
