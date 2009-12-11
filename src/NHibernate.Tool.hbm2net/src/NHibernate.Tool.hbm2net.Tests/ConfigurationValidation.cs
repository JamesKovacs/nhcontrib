using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml;

namespace NHibernate.Tool.hbm2net.Tests
{
    [TestFixture]
    public class ConfigurationValidation
    {
        [Test]
        [ExpectedException(typeof(ConfigurationValidationException))]
        public void ShouldThrowIfRootNodeIsWrong()
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><codegenWrong><generate renderer=\"NHibernate.Tool.hbm2net.T4.T4Render, NHibernate.Tool.hbm2net\" package=\"\"><param name=\"template\">res://NHibernate.Tool.hbm2net.templates.hbm2net.tt</param><param name=\"output\">clazz.GeneratedName+\".generated.cs\"</param></generate></codegenWrong>");
                new ConfigurationValidator().Validate(doc);
            }
            catch (ConfigurationValidationException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        [Test]
        [ExpectedException(typeof(ConfigurationValidationException))]
        public void ShouldThrowIfNothingToGenerate()
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><codegen></codegen>");
                new ConfigurationValidator().Validate(doc);
            }
            catch (ConfigurationValidationException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        [Test]
        [ExpectedException(typeof(ConfigurationValidationException))]
        public void ShouldThrowIfGeneratorNameIsMissing()
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><codegen><generate  package=\"\"><param name=\"template\">res://NHibernate.Tool.hbm2net.templates.hbm2net.tt</param><param name=\"output\">clazz.GeneratedName+\".generated.cs\"</param></generate></codegen>");
                new ConfigurationValidator().Validate(doc);
            }
            catch (ConfigurationValidationException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        [Test]
        [ExpectedException(typeof(ConfigurationValidationException))]
        public void ShouldThrowIfGeneratorNameIsWrong()
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><codegen><generateWrong renderer=\"\" package=\"\"><param name=\"template\">res://NHibernate.Tool.hbm2net.templates.hbm2net.tt</param><param name=\"output\">clazz.GeneratedName+\".generated.cs\"</param></generateWrong></codegen>");
                new ConfigurationValidator().Validate(doc);
            }
            catch (ConfigurationValidationException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        [Test]
        [ExpectedException(typeof(ConfigurationValidationException))]
        public void ShouldThrowIfParameterNameIsMissing()
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><codegen><generate renderer=\"\" package=\"\"><param >res://NHibernate.Tool.hbm2net.templates.hbm2net.tt</param><param name=\"output\">clazz.GeneratedName+\".generated.cs\"</param></generate></codegen>");
                new ConfigurationValidator().Validate(doc);
            }
            catch (ConfigurationValidationException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        [Test]
        public void ShouldAcceptOptionalPackageAttribute()
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><codegen><generate renderer=\"\" ><param name=\"template\">res://NHibernate.Tool.hbm2net.templates.hbm2net.tt</param><param name=\"output\">clazz.GeneratedName+\".generated.cs\"</param></generate></codegen>");
                new ConfigurationValidator().Validate(doc);
            }
            catch (ConfigurationValidationException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        [Test]
        public void ShouldAcceptNoParameters()
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><codegen><generate renderer=\"\" ></generate></codegen>");
                new ConfigurationValidator().Validate(doc);
            }
            catch (ConfigurationValidationException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        [Test]
        public void ShouldAcceptMoreThanOneGenerateNode()
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><codegen><generate renderer=\"\" ></generate><generate renderer=\"\" ></generate><generate renderer=\"\" ></generate></codegen>");
                new ConfigurationValidator().Validate(doc);
            }
            catch (ConfigurationValidationException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
