using System.Reflection;
using NUnit.Framework;
using SolrNet.Attributes;
using SolrNet.Exceptions;

namespace SolrNet.Tests {
	[TestFixture]
	public class UniquePropertyFinderTests {
		public class DocWithoutUniqueKey : ISolrDocument {
			public int id {
				get { return 0; }
			}
		}

		public class DocWithTwoUniqueKeys : ISolrDocument {
			[SolrUniqueKey]
			public string id {
				get { return ""; }
			}

			[SolrUniqueKey]
			public string id2 {
				get { return ""; }
			}
		}

		public class DocWithOneUniqueKey : ISolrDocument {
			[SolrUniqueKey]
			public string id {
				get { return ""; }
			}

			public string id2 {
				get { return ""; }
			}
		}

		public class DocWithOneUniqueKey2 : ISolrDocument {
			public string id {
				get { return ""; }
			}

			[SolrUniqueKey]
			public string id2 {
				get { return ""; }
			}
		}

		[Test]
		public void FindOneUniqueKey() {
			IUniqueKeyFinder<DocWithOneUniqueKey> f = new UniqueKeyFinder<DocWithOneUniqueKey>();
			Assert.AreEqual("id", f.UniqueKeyProperty.Name);
		}

		[Test]
		public void FindOneUniqueKey_InAnyOrder() {
			IUniqueKeyFinder<DocWithOneUniqueKey2> f = new UniqueKeyFinder<DocWithOneUniqueKey2>();
			Assert.AreEqual("id2", f.UniqueKeyProperty.Name);
		}

		[Test]
		public void NoUniqueKey() {
			IUniqueKeyFinder<DocWithoutUniqueKey> f = new UniqueKeyFinder<DocWithoutUniqueKey>();
			Assert.IsNull(f.UniqueKeyProperty);
		}

		[Test]
		[ExpectedException(typeof (BadMappingException))]
		public void ClassWithTwoUniqueKeys_ShouldFail() {
			IUniqueKeyFinder<DocWithTwoUniqueKeys> f = new UniqueKeyFinder<DocWithTwoUniqueKeys>();
			PropertyInfo info = f.UniqueKeyProperty;
		}

		[Test]
		[ExpectedException(typeof (BadMappingException))]
		public void ClassWithTwoUniqueKeys_ShouldFailEveryTime() {
			IUniqueKeyFinder<DocWithTwoUniqueKeys> f = new UniqueKeyFinder<DocWithTwoUniqueKeys>();
			try {
				PropertyInfo info = f.UniqueKeyProperty;
			} catch (BadMappingException) {}
			PropertyInfo info2 = f.UniqueKeyProperty;
		}
	}
}