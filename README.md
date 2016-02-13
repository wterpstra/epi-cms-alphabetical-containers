# epi-cms-date-containers
Adds support for auto-generated date containers

# usage
* Create a data container content type and derive it from IDateContainer
* Create the content type that needs to be placed in the generated date container. This type should derive from IHasDateContainer
* Implement the ConstructDateContainer method. This method should construct the date container (it should not persist it)
