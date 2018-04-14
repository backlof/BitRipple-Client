Todo
==

- [ ] Create an MVVM application for editing data like `forms`
- [ ] Use [Rhino Mocks](https://www.hibernatingrhinos.com/oss/rhino-mocks) and DI to unit test functionality
- [ ] Move settings out of BitRippleService and into the individual applications
- [ ] Move utilities into its own DLL-project
- [ ] Move repositories into its own DLL-project
- [ ] Applications should only have direct access to services. Services should resolve DI internally
- [ ] Services should only expose DTO-models
- [ ] Move helper functionality out of DTO models and into the specific utilities they are necessary for (unless they are general)