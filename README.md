# AGLPets

This is a VisualStudio 2015 solution for retrieving a list of owners and their pets from a webservice, displaying the pet names filtered by pet type, sorted alphabetically and grouped by the owner's gender.

The solution consists of a thin MVC client that accesses a WebAPI proxy for retrieving the list of owners/pets from the AGL webservice - this enables decoupling the portal(s) from the AGL webservice. Furthermore, other type of portals (mobile, etc...) can access the same WebAPI proxy with minimum changes to the way it is accessed by the MVC portal.

The WebAPI (PetsManager) implements an interface (PetsManagerContracts) that retrieves a list of owners/pets from AGL, filters it by pet type and flattens it into a list of DTOs that is consumed by the client. This reduces network traffic between client and the WebAPI, leaving to the client the task to display it as it seems fit. The interface can be easily extended to provide clients with other methods that return different lists of DTOs.

The DTOs are defined in a different project so to further decouple portal(s) and WebAPI proxy. The Utilities folder contains projects that are common to clients and services within the solution.

The portal and WebAPI projects use Dependency Injection (Unity) to provide IOC and facilitate Unit Testing (NUnit). Since the HttpClient class does not have an interface (due to it being an abstraction layer over another HTTP library), mocking it has to be done on the HttpMessageHandler class level, by setting the desired response on the HttpMessageResponse.

Future areas for improvement and refactoring have been marked throughout the code with the //TODO attribute.
