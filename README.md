# DatabaseOperations

An operations library for use on any MS SQL Server database.

## Introduction

I was required to automate a database backup service for our deployment solution at my place of work.  I found the code on how to do this on the Microsoft Docs site.  What I didn't locate was any libraries based on this code.  So, I made this library that I am now putting out there for other people to use.

## Usages

I will show you how to use the library here.

## Wrappers

I made some wrappers for the concrete Microsoft SQL classes that didn't have any interfaces.  The reason for this is so that I could test the code.  When you look at any legacy code, you think of ways to make it testable before making any refactorings.  So, I made the code on the Microsoft Docs testable.

## Road Map

1. Finish the initial backup operation.
2. Add the restore operation.
