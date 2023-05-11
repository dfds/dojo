# Kubernetes learning path

Difficulty: Entry level to intermediate.

## Topics

- Prereq skills
  - Docker build, ship, run
  - Linux, command line
- Architecture and core concepts
  - API server: https://kubernetes.io/docs/concepts/overview/components/#kube-apiserver
  - etcd: https://kubernetes.io/docs/concepts/overview/components/#etcd
  - Kube Scheduler: https://kubernetes.io/docs/concepts/overview/components/#kube-scheduler
  - Kube Proxy: https://kubernetes.io/docs/concepts/overview/components/#kube-proxy
  - Kubelet: https://kubernetes.io/docs/concepts/overview/components/#kubelet
  - CoreDNS: https://kubernetes.io/docs/concepts/overview/components/#dns
  - Container runtime: https://kubernetes.io/docs/concepts/overview/components/#container-runtime
  - Kubeconfig: https://kubernetes.io/docs/tasks/access-application-cluster/configure-access-multiple-clusters/
  - Namespaces: https://kubernetes.io/docs/concepts/overview/working-with-objects/namespaces/
  - kubectl: https://kubernetes.io/docs/reference/kubectl/
  - k9s: https://k9scli.io/
  - Awareness of mainstream Kubernetes distributions (AKS, EKS, GKS, Rancher, kubeadm, k3s, kind)
- Scheduling
  - Pods: https://kubernetes.io/docs/concepts/workloads/pods/
  - ReplicaSets: https://kubernetes.io/docs/concepts/workloads/controllers/replicaset/
  - Deployments: https://kubernetes.io/docs/concepts/workloads/controllers/deployment/
  - StatefulSets: https://kubernetes.io/docs/concepts/workloads/controllers/statefulset/
  - DaemonSets: https://kubernetes.io/docs/concepts/workloads/controllers/daemonset/
- Service Networking
  - Services: https://kubernetes.io/docs/concepts/services-networking/service/
  - Ingress: https://kubernetes.io/docs/concepts/services-networking/ingress/
  - IngressRoute (Traefik specific): https://doc.traefik.io/traefik/routing/providers/kubernetes-crd/
  - Load Balancer (AWS specific): https://docs.aws.amazon.com/elasticloadbalancing/latest/application/application-load-balancers.html
- Logging
  - Pod logs: https://kubernetes.io/docs/tasks/debug/debug-application/debug-running-pod/
- Security
  - ServiceAccounts: https://kubernetes.io/docs/concepts/security/service-accounts/
  - Roles and ClusterRoles: https://kubernetes.io/docs/reference/access-authn-authz/rbac/#role-and-clusterrole
  - RoleBindings and ClusterRoleBindings: https://kubernetes.io/docs/reference/access-authn-authz/rbac/#rolebinding-and-clusterrolebinding
  - Pod Security Standards: https://kubernetes.io/docs/concepts/security/pod-security-standards/
  - Pod Security Admission: https://kubernetes.io/docs/concepts/security/pod-security-admission/
  - IRSA: IAM Roles for ServiceAccounts (AWS specific): https://docs.aws.amazon.com/eks/latest/userguide/associate-service-account-role.html
- Storage
  - StorageClasses (focus on AWS specific ones like csi-gp3): https://kubernetes.io/docs/concepts/storage/storage-classes/ and https://docs.aws.amazon.com/eks/latest/userguide/ebs-csi.html
  - PersistentVolumes: https://kubernetes.io/docs/concepts/storage/persistent-volumes/
  - PersistentVolumeClaims: https://kubernetes.io/docs/tasks/configure-pod-container/configure-persistent-volume-storage/#create-a-persistentvolumeclaim
  - Volumes: https://kubernetes.io/docs/tasks/configure-pod-container/configure-persistent-volume-storage/#create-a-pod
- Configuration
  - Secrets: https://kubernetes.io/docs/concepts/configuration/secret/
  - ConfigMaps: https://kubernetes.io/docs/concepts/configuration/configmap/
- Troubleshooting:
  - Debug Pods: https://kubernetes.io/docs/tasks/debug/debug-application/debug-pods/
  - Debug Services: https://kubernetes.io/docs/tasks/debug/debug-application/debug-service/
  - Debug Running Pods: https://kubernetes.io/docs/tasks/debug/debug-application/debug-running-pod/
  - Get a Shell to a Running Container: https://kubernetes.io/docs/tasks/debug/debug-application/get-shell-running-container/

## Suggested training

- "Kubernetes for the Absolute Beginners - Hands-on" on Udemy.
- "Certified Kubernetes Administrator (CKA) with Practice Tests" on Udemy.
