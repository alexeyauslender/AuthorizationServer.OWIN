AuthorizationServer.OWIN
===============================

Implementation of an OAuth2 authorization server using Katana project (OWIN specification).

Authorization server is the infrastructure for implementing application and API authorization.
OAuth 2.0 focuses on client developer simplicity while providing specific authorization flows for web
applications, desktop applications, mobile phones, and living room devices http://oauth.net/2/.
Instead of using the credentials such as user name and password to access a protected resource,the client obtains an access token.
To request an access token, the client obtains authorization from the resource owner.The authorization is expressed in the form of an
authorization grant, which the client uses to request the access
token.  OAuth defines four grant types: authorization code, implicit,
resource owner password credentials, and client credentials.

Project description
===================
This project includes Authorization Server implementation using Microsoft Katana project.Implementation of four authorization grants of OAuth 2.0 also included.


Project setup
=============
Prerequisites:
Visual Studio 2013,Microsoft.Net 4.5,Microsoft SQL Server 2012,IIS express (comes with VS installation).

Download the zip file from https://github.com/aus1977/Alexus.AuthorizationServer.OWIN/archive/Integration.zip.
Open AuthorizationServer.sln using Visual Studio 2013.Open Package Manager Console,"Restore" button will appear on right top corner.Restore missing packages by pressing Restore.Compile the project.Open solution properties 
by pressing on Properties tab on AuthorizationServer solution context menu in Solution Explorer.Choose Multiple startup projects
select Start entry in Action drop down list for ResourceServer,AuthorizationServer and one of desired grant types.Press F5 and enjoy the power of OAuth 2.0.
