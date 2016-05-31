# epi-cms-alphabetical-containers
Adds support for auto-generated alphabetical containers

# usage
* Create a data container content type and derive it from IAlphabeticalContainer
* Create the content type that needs to be placed in the generated Alphabetical container. This type should derive from IHasAlphabeticalContainer
* Implement the ConstructAlphabeticalContainer method. This method should construct the alphabetical container (it should not persist it)
