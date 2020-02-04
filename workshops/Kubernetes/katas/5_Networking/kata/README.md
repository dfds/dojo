DFDS Kubernetes Training - Code kata #5
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:

### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

By default, Kubernetes pods have unrestricted network access both inside and outside the cluster. However, it is often desirable to restrict network access to and from pods, particularly for security reasons. Kubernetes NetworkPolicies provide a flexible way to implement these networking restrictions, giving you control over all of the network traffic involving your pods. In this lab, walk through NetworkPolicies concepts, and learn how to create a network policy that can control access to a MySQL pod.


### 1. Create your project directory
`mkdir /kata5`<br/>
`cd /kata5`

### 2. Create a NetworkPolicy descriptor file to allocate some storage in our cluster
Create a file named "mysql-network-policy.yml" and add a NetworkPolicy definition:

```
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: mysql-network-policy
  namespace: default
spec:
  podSelector:
    matchLabels:
      role: db
  policyTypes:
  - Ingress
  - Egress
  ingress:
  - from:
    - ipBlock:
        cidr: 172.17.0.0/16
        except:
        - 172.17.1.0/24
    - namespaceSelector:
        matchLabels:
          project: my-solution
    - podSelector:
        matchLabels:
          role: api
    ports:
    - protocol: TCP
      port: 6379
  egress:
  - to:
    - ipBlock:
        cidr: 10.0.0.0/24
    ports:
    - protocol: TCP
      port: 5978
```




## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 