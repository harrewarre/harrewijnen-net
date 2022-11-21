{
  "title": "Today I learned - kubectl replace",
  "summary": "Want to quickly edit something running on kubernetes? Here's a nice (bit reckless) way to do so.",
  "slug": "til-k8s-replace",
  "tags": [
    "today-i-learned",
    "kubernetes",
    "kubectl"
  ],
  "created": "2022-11-21"
}

---

# Today I learned - kubectl replace

Here's one for your toolbox, but use with care. Say you have an app running on kubernetes in a pod and you spot something you want to change. This is one way to get it done.

For the example, I spun up an nginx server called `webapp`.

The command to edit a resource running on kubernetes is `kubectl edit <thing> <name>`. So here goes:

    kubectl edit pod webapp

You'll see a whole bunch of yaml that represents the pods definition. In what editor will depend on what you've set as your default editor for any `kubectl` related business. (See: `KUBE_EDITOR` environment variable).

    
    # Please edit the object below. Lines beginning with a '#' will be ignored,
    # and an empty file will abort the edit. If an error occurs while saving this file will be
    # reopened with the relevant failures.
    #
    apiVersion: v1
    kind: Pod
    metadata:
    creationTimestamp: "2022-11-21T18:51:24Z"
    labels:
      run: webapp
    name: webapp
    namespace: default
    spec:
      containers:
      - image: nginx
        name: webapp
      
<small>Shortened for sanity</small>

Now you can make any changes you want and when you save them, they'll get applied to the resource you started editting. Not so fast though. You're not allowed to just update anything and apply it!


    resources:
      requests:
        memory: 128M
      limits:
        memory: 512M

If for example you try to set resource limits you'll get an error.

    # pods "webapp" was not valid:
    # * spec: Forbidden: pod updates may not change fields other than `spec.containers[*].image`, `spec.initContainers[*].image`, `spec.activeDeadlineSeconds`... <snipped>

If you close out of the editor and go back to the CLI you'll see this message:


    A copy of your changes has been stored to "/tmp/kubectl-edit-4134617318.yaml"
    error: Edit cancelled, no valid changes were saved.

See that? They saved our changes in a temporary file. Let's say your really really wanted to apply these changes. Here you go:

    kubectl replace -f /tmp/kubectl-edit-4134617318.yaml --force

Doing the above will cause two things. The first is: **it will delete the running pod we had in the definition**. The second thing is, **it will replace** the thing we had with whatever we have in our temporary definition.

    pod "webapp" deleted
    pod/webapp replaced

As you can see, it's quite destructive, but for local development and a bit of hacking around this is a nice quick and dirty way to recreate something with different parameters quickly.