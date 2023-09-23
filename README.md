# BestStories_Assignment
Best Stories Assignment

How to run:

cd .\BestStoriesAPI\
dotnet run If you get https related error, run:
dotnet dev-certs https --trust

Assumptions:

1. Auth is not required.
2. I assume that ranking might be updated quite frequently set different cache retention times for those different objects.


Things to improve given a time:

1. Better test coverage. At the moment only service layer is tested.
