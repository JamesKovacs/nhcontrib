This is a copy of popular NHibernate Enterprise Sample from the page http://www.codeproject.com/KB/architecture/NHibernateBestPractices.aspx

Changes:

14-04-2008:
- updated to NH 2.0
- removed use of Castle facilities.
- updated some libraries.
- implemented session manager of Burrow.

note: I haven't uploaded config files to svn but I added templates of them. When you do checkout (first time), then you should create these files (simply copy the template files without the extension '.template' and review the paths)

23-04-2008
- DAOs allow multi-database
- Lazy load for Customer.Orders