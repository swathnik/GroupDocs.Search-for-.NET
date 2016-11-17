﻿Imports Aspose.Email.Outlook.Pst

Public Class Searching
    ''' <summary>
    ''' Creates index, adds documents to index and search string in index
    ''' </summary>
    ''' <param name="searchString">string to search</param>
    Public Shared Sub SimpleSearch(searchString As String)
        'ExStart:SimpleSearch
        ' Create index
        Dim index As New Index(Utilities.indexPath)

        ' Add documents to index
        index.AddToIndex(Utilities.documentsPath)

        ' Search in index
        Dim searchResults As SearchResults = index.Search(searchString)

        ' List of found files
        For Each documentResultInfo As DocumentResultInfo In searchResults
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", searchString, documentResultInfo.HitCount, documentResultInfo.FileName)
        Next
        'ExEnd:SimpleSearch
    End Sub

    ''' <summary>
    ''' Creates index, adds documents to index and do boolean search
    ''' </summary>
    ''' <param name="firstTerm">first term to search</param>
    ''' <param name="secondTerm">second term to search</param>
    Public Shared Sub BooleanSearch(firstTerm As String, secondTerm As String)
        'ExStart:BooleanSearch
        ' Create index
        Dim index As New Index(Utilities.indexPath)

        ' Add documents to index
        index.AddToIndex(Utilities.documentsPath)

        ' Search in index
        Dim searchResults As SearchResults = index.Search(Convert.ToString(firstTerm & Convert.ToString("OR")) & secondTerm)

        ' List of found files
        For Each documentResultInfo As DocumentResultInfo In searchResults
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", firstTerm, documentResultInfo.HitCount, documentResultInfo.FileName)
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", secondTerm, documentResultInfo.HitCount, documentResultInfo.FileName)
        Next
        'ExEnd:BooleanSearch
    End Sub

    ''' <summary>
    ''' Creates index, adds documents to index and do regex search
    ''' </summary>
    ''' <param name="relevantKey">single keyword</param>
    ''' <param name="regexString">regex</param>
    Public Shared Sub RegexSearch(relevantKey As String, regexString As String)
        'ExStart:Regexsearch
        ' Create index
        Dim index As New Index(Utilities.indexPath)

        ' Add documents to index
        index.AddToIndex(Utilities.documentsPath)

        ' Search for documents where at least one word contain given regex
        Dim searchResults1 As SearchResults = index.Search(relevantKey)

        'Search for documents where present term1 or any email adress or term2
        Dim searchResults2 As SearchResults = index.Search(regexString)

        ' List of found files 
        Console.WriteLine("Follwoing document(s) contain provided relevant tag: " & vbLf)
        For Each documentResultInfo As DocumentResultInfo In searchResults1
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", relevantKey, documentResultInfo.HitCount, documentResultInfo.FileName)
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", regexString, documentResultInfo.HitCount, documentResultInfo.FileName)
        Next

        ' List of found files
        Console.WriteLine("Follwoing document(s) contain provided RegEx: " & vbLf)
        For Each documentResultInfo As DocumentResultInfo In searchResults2
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", relevantKey, documentResultInfo.HitCount, documentResultInfo.FileName)
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", regexString, documentResultInfo.HitCount, documentResultInfo.FileName)
        Next
        'ExEnd:Regexsearch
    End Sub

    ''' <summary>
    ''' Creates index, 
    ''' Adds documents to index 
    ''' Enable fuzzy search
    ''' Set similarity level from 0.0 to 1.0
    ''' Do Fuzzy search
    ''' </summary>
    ''' <param name="searchString">Misspelled string</param>
    ''' 
    Public Shared Sub FuzzySearch(searchString As String)
        'ExStart:Fuzzysearch
        Dim index As New Index(Utilities.indexPath)
        index.AddToIndex(Utilities.documentsPath)

        Dim parameters As New SearchParameters()
        ' turning on Fuzzy search feature
        parameters.FuzzySearch.Enabled = True

        ' set low similarity level to search for less similar words and get more results
        parameters.FuzzySearch.SimilarityLevel = 0.1
        Dim lessSimilarResults As SearchResults = index.Search(searchString, parameters)
        Console.WriteLine("Results with less similarity level that is currently set to =" + parameters.FuzzySearch.SimilarityLevel)
        For Each lessSimilarResultsDoc As DocumentResultInfo In lessSimilarResults
            Console.WriteLine(lessSimilarResultsDoc.FileName + vbLf)
        Next

        ' set high similarity level to search for more similar words and get less results
        parameters.FuzzySearch.SimilarityLevel = 0.9
        Dim moreSimilarResults As SearchResults = index.Search(searchString, parameters)

        Console.WriteLine("Results with high similarity level that is currently set to =" + parameters.FuzzySearch.SimilarityLevel)
        For Each highSimilarityLevelDoc As DocumentResultInfo In moreSimilarResults
            Console.WriteLine(highSimilarityLevelDoc.FileName + vbLf)
        Next
        'ExEnd:Fuzzysearch
    End Sub

    ''' <summary>
    ''' Creates index, adds documents to index and do faceted search
    ''' </summary>
    ''' <param name="searchString">search string</param>
    Public Shared Sub FacetedSearch(searchString As String)
        'ExStart:Facetedsearch
        ' Create index
        Dim index As New Index(Utilities.indexPath)

        ' Add documents to index
        index.AddToIndex(Utilities.documentsPath)

        ' Searching for any document in index that contain word "return" in file content
        Dim searchResults As SearchResults = index.Search(Convert.ToString("Content:") & searchString)


        ' List of found files
        For Each documentResultInfo As DocumentResultInfo In searchResults
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", searchString, documentResultInfo.HitCount, documentResultInfo.FileName)
        Next
        'ExEnd:Facetedsearch
    End Sub

    ''' <summary>
    ''' Gets list of the words in found documents that matched the search query
    ''' </summary>
    ''' <param name="searchString">Search string</param>
    ''' 
    Public Shared Sub GetMatchingWordsInFuzzySearchResult(searchString As String)
        'ExStart:GetMatchingWordsInFuzzySearchResult
        Dim index As New Index(Utilities.indexPath)
        index.AddToIndex(Utilities.documentsPath)

        Dim parameters As New SearchParameters()
        ' turning on Fuzzy search feature
        parameters.FuzzySearch.Enabled = True

        ' set low similarity level to search for less similar words and get more results
        parameters.FuzzySearch.SimilarityLevel = 0.2

        Dim fuzzySearchResults As SearchResults = index.Search(searchString, parameters)
        For Each documentResultInfo As DocumentResultInfo In fuzzySearchResults
            Console.WriteLine("Document {0} was found with query ""{1}""" & vbLf & "Words list that was found in document:", documentResultInfo.FileName, searchString)
            For Each term As String In documentResultInfo.Terms
                Console.Write("{0}; ", term)
            Next
            Console.WriteLine()
        Next
        'ExEnd:GetMatchingWordsInFuzzySearchResult
    End Sub

    ''' <summary>
    ''' Gets list of the words in found documents that matched the search query
    ''' </summary>
    ''' <param name="searchString">Search string</param>
    ''' 
    Public Shared Sub GetMatchingWordsInRegexSearchResult(searchString As String)
        'ExStart:GetMatchingWordsInRegexSearchResult
        Dim index As New Index(Utilities.indexPath)
        index.AddToIndex(Utilities.documentsPath)

        Dim parameters As New SearchParameters()

        Dim regexSearchResults As SearchResults = index.Search(searchString)

        For Each documentResultInfo As DocumentResultInfo In regexSearchResults
            Console.WriteLine("Document {0} was found with query ""{1}""" & vbLf & "Words list that was found in document:", documentResultInfo.FileName, regexSearchResults)
            For Each term As String In documentResultInfo.Terms
                Console.Write("{0}; ", term)
            Next
            Console.WriteLine()
        Next
        'ExEnd:GetMatchingWordsInRegexSearchResult
    End Sub

    ''' <summary>
    ''' Creates index, adds documents to index and searches file name that containes similar/inputted string 
    ''' </summary>
    ''' <param name="searchString">search string</param>
    Public Shared Sub SearchFileName(searchString As String)
        'ExStart:SearchFileName
        ' Create index
        Dim index As New Index(Utilities.indexPath)

        ' Add documents to index
        index.AddToIndex(Utilities.documentsPath)

        ' Searching for any document in index that contain search string in file name
        Dim searchResults As SearchResults = index.Search(Convert.ToString("FileName:") & searchString)


        ' List of found files
        For Each documentResultInfo As DocumentResultInfo In searchResults
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", searchString, documentResultInfo.HitCount, documentResultInfo.FileName)
        Next
        'ExEnd:SearchFileName
    End Sub

    ''' <summary>
    ''' Creates index, adds documents to index and do faceted search combine with boolean search
    ''' </summary>
    ''' <param name="firstTerm">first term</param>
    ''' <param name="secondTerm">second term</param>
    Public Shared Sub FacetedSearchWithBooleanSearch(firstTerm As String, secondTerm As String)
        'ExStart:FacetedSearchWithBooleanSearch
        ' Create index
        Dim index As New Index(Utilities.indexPath)

        ' Add documents to index
        index.AddToIndex(Utilities.documentsPath)
        'Faceted search combine with boolean search
        Dim searchResults As SearchResults = index.Search(Convert.ToString((Convert.ToString("Content:") & firstTerm) + "OR Content:") & secondTerm)

        ' List of found files
        For Each documentResultInfo As DocumentResultInfo In searchResults
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", firstTerm, documentResultInfo.HitCount, documentResultInfo.FileName)
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", secondTerm, documentResultInfo.HitCount, documentResultInfo.FileName)
        Next
        'ExEnd:FacetedSearchWithBooleanSearch
    End Sub

    ''' <summary>
    ''' Creates index, adds documents to index and do search on the basis of synonyms by turning synonym search true
    ''' </summary>
    ''' <param name="searchString">string to search</param>
    Public Shared Sub SynonymSearch(searchString As String)
        'ExStart:SynonymSearch
        ' Create or load index
        Dim index As New Index(Utilities.indexPath)

        ' load synonyms
        index.LoadSynonyms(Utilities.synonymFilePath)

        index.AddToIndex(Utilities.documentsPath)

        ' Turning on synonym search feature
        Dim parameters As New SearchParameters()
        parameters.UseSynonymSearch = True

        ' searching for documents with words one of words "remote", "virtual" or "online"
        Dim searchResults As SearchResults = index.Search(searchString, parameters)

        ' List of found files
        For Each documentResultInfo As DocumentResultInfo In searchResults
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", searchString, documentResultInfo.HitCount, documentResultInfo.FileName)
        Next
        'ExEnd:SynonymSearch
    End Sub

    ''' <summary>
    ''' Searches documents wih exact phrase 
    ''' </summary>
    ''' <param name="searchString">string to search</param>
    Public Shared Sub ExactPhraseSearch(searchString As String)
        'ExStart:ExactPhraseSearch
        ' Create or load index
        Dim index As New Index(Utilities.indexPath, True)

        index.AddToIndex(Utilities.documentsPath)

        Dim searchResults As SearchResults = index.Search(searchString)

        ' List of found files
        For Each documentResultInfo As DocumentResultInfo In searchResults
            Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", searchString, documentResultInfo.HitCount, documentResultInfo.FileName)
        Next
        'ExEnd:ExactPhraseSearch
    End Sub

    ''' <summary>
    ''' Performs a case sensitive search
    ''' </summary>
    ''' <param name="caseSensitiveSearchQuery">string to search</param>
    Public Shared Sub CaseSensitiveSearch(caseSensitiveSearchQuery As String)
        'ExStart:CaseSensitiveSearch
        Dim inMemoryIndex As Boolean = False
        Dim caseSensitive As Boolean = True
        Dim settings As New IndexingSettings(inMemoryIndex, caseSensitive)

        ' Create or load index
        Dim index As New Index(Utilities.indexPath, settings)

        index.AddToIndex(Utilities.documentsPath)

        Dim parameters As New SearchParameters()
        parameters.UseCaseSensitiveSearch = True
        ' using case sensitive search feature
        Dim searchResults As SearchResults = index.Search(caseSensitiveSearchQuery, parameters)

        If searchResults.Count > 0 Then
            ' List of found files
            For Each documentResultInfo As DocumentResultInfo In searchResults
                Console.WriteLine("Query ""{0}"" has {1} hit count in file: {2}", caseSensitiveSearchQuery, documentResultInfo.HitCount, documentResultInfo.FileName)
            Next
        Else
            Console.WriteLine("No results found")
        End If
        'ExEnd:CaseSensitiveSearch
    End Sub

    ''' <summary>
    ''' Shows how to implement own custom extractor for outlook document for the extension .ost and .pst files
    ''' </summary>
    ''' <param name="searchString">string to search</param>
    Public Shared Sub OwnExtractorOst(searchString As String)
        'ExStart:OwnExtractorOst
        ' Create or load index
        Dim index As New Index(Utilities.indexPath)

        index.CustomExtractors.Add(New CustomOstPstExtractor())
        ' Adding new custom extractor for container document
        index.AddToIndex(Utilities.documentsPath)
        ' Documents with "ost" and "pst" extension will be indexed using MyCustomContainerExtractor
        Dim searchResults As SearchResults = index.Search(searchString)
        'ExEnd:OwnExtractorOst
    End Sub

    ''' <summary>
    ''' Shows how to implement own custom extractor for outlook document for the extension .ost and .pst files
    ''' </summary>
    ''' <param name="searchString">string to search</param>
    Public Shared Sub DetailedResults(searchString As String)
        'ExStart:DetailedResultsPropertyInDocuments
        ' Create or load index
        Dim index As New Index(Utilities.indexPath)
        index.AddToIndex(Utilities.documentsPath)

        Dim results As SearchResults = index.Search(searchString)

        For Each resultInfo As DocumentResultInfo In results
            If resultInfo.DocumentType = DocumentType.OutlookEmailMessage Then
                ' for email message result info user should cast resultInfo as OutlookEmailMessageResultInfo for acessing EntryIdString property
                Dim emailResultInfo As OutlookEmailMessageResultInfo = TryCast(resultInfo, OutlookEmailMessageResultInfo)

                Console.WriteLine("Query ""{0}"" has {1} hit count in message {2} in file {3}", searchString, emailResultInfo.HitCount, emailResultInfo.EntryIdString, emailResultInfo.FileName)
            Else
                Console.WriteLine("Query ""{0}"" has {1} hit count in file {2}", searchString, resultInfo.HitCount, resultInfo.FileName)
            End If

            For Each detailedResult As DetailedResultInfo In resultInfo.DetailedResults
                Console.WriteLine("{0}In field ""{1}"" there was found {2} hit count", vbTab, detailedResult.FieldName, detailedResult.HitCount)
            Next
        Next
        'ExEnd:DetailedResultsPropertyInDocuments
    End Sub

    ''' <summary>
    ''' Gives warnings if try to run Search with options that are not supported in index
    ''' </summary>
    ''' <param name="searchString">string to search</param>
    Public Shared Sub NotSupportedOptionWarning(searchString As String)
        'ExStart:NotSupportedOptionWarning
        'create index
        Dim index As New Index(Utilities.indexPath)
        ' index.IndexingSettings.QuickIndexing = true;
        AddHandler index.ErrorHappened, AddressOf index_ErrorHappened
        ' QuickIndex ad = new QuickIndex();
        index.AddToIndex(Utilities.documentsPath)

        Dim fuzzySearchParameters As New SearchParameters()
        fuzzySearchParameters.FuzzySearch.Enabled = True

        ' Run fuzzy search
        Dim results As SearchResults = index.Search(searchString, fuzzySearchParameters)

        ' Run regex search
        Dim regexString As String = "dropbox ^[A-Z0-9._%+\-|A-Z0-9._%+-]+@++[A-Z0-9.\-|A-Z0-9.-]+\.[A-Z|A-Z]{2,}$ folder"
        Dim results1 As SearchResults = index.Search(regexString)

        Dim synonymSearchParameters As New SearchParameters()
        synonymSearchParameters.UseSynonymSearch = True

        ' Run synonym search without loaded synonyms
        Dim results2 As SearchResults = index.Search(searchString, synonymSearchParameters)

        'ExEnd:NotSupportedOptionWarning

    End Sub

    'ExStart:index_ErrorHappened
    ''' <summary>
    ''' Event Handler for search options not supported in index
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Shared Sub index_ErrorHappened(sender As Object, e As Search.Events.BaseIndexArg)
        ' e.Message contains corresponding message 
        'if search option is not supported
        'string notificationMessage = e.Message;
        Console.WriteLine(e.Message)
    End Sub
    'ExEnd:index_ErrorHappened
    ''' <summary>
    ''' Gets total hits count
    ''' </summary>
    ''' <param name="searchString">string to search</param> 
    Public Shared Sub GetTotalHitCount(searchString As String)

        'ExStart:GetTotalHitCount
        Dim index As New Index(Utilities.indexPath)
        index.AddToIndex(Utilities.documentsPath)

        Dim results As SearchResults = index.Search(searchString)
        Console.WriteLine("Searching with query ""{0}"" returns {1} documents with {2} total hit count", searchString, results.Count, results.TotalHitCount)
        'ExEnd:GetTotalHitCount
    End Sub

    Public Sub OpenFoundMessageUsingAsposeEmail(searchString As String)
        Dim myPstFile As String = Utilities.pathToPstFile


        ' Indexing MS Outlook storage with email messages
        Dim index As New Index(Utilities.indexPath)
        AddHandler index.OperationFinished, Utilities.index_OperationFinished
        index.AddToIndex(myPstFile)

        ' Searching in index
        Dim results As SearchResults = index.Search(searchString)

        ' User gets all messages that qualify to search query using Aspose.Email API
        Dim messages As New MessageInfoCollection()
        For Each searchResult As DocumentResultInfo In results
            If searchResult.DocumentType = DocumentType.OutlookEmailMessage Then
                Dim emailResultInfo As OutlookEmailMessageResultInfo = TryCast(searchResult, OutlookEmailMessageResultInfo)
                Dim message As MessageInfo = GetEmailMessagesById(Utilities.pathToPstFile, emailResultInfo.EntryIdString)
                If message IsNot Nothing Then
                    messages.Add(message)
                End If
            End If
        Next
    End Sub

    Private Function GetEmailMessagesById(fileName As String, fieldId As String) As MessageInfo
        Dim pst As PersonalStorage = PersonalStorage.FromFile(fileName, False)
        Return GetEmailMessagesById(pst.RootFolder, fieldId)
    End Function

    Private Function GetEmailMessagesById(folderInfo As FolderInfo, fieldId As String) As MessageInfo
        Dim result As MessageInfo = Nothing
        Dim messageInfoCollection As MessageInfoCollection = folderInfo.GetContents()
        For Each messageInfo As MessageInfo In messageInfoCollection
            If messageInfo.EntryIdString = fieldId Then
                result = messageInfo
                Exit For
            End If
        Next

        If result Is Nothing AndAlso folderInfo.HasSubFolders Then
            For Each subfolderInfo As FolderInfo In folderInfo.GetSubFolders()
                result = GetEmailMessagesById(subfolderInfo, fieldId)
                If result IsNot Nothing Then
                    Exit For
                End If
            Next
        End If
        Return result
    End Function

End Class
