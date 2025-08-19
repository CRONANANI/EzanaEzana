// Research Tools JavaScript
// API Base URL
const API_BASE_URL = '/api';

// Global variables
let currentData = null;
let filteredData = null;
let currentDataView = null;
let followedCongressPeople = new Set();

// Initialize the page
document.addEventListener('DOMContentLoaded', function() {
    console.log('Research Tools page loaded');
    
    // Load any saved followed congresspeople
    const saved = localStorage.getItem('followedCongressPeople');
    if (saved) {
        followedCongressPeople = new Set(JSON.parse(saved));
    }
});

// Toggle card expansion
function toggleCardExpansion(dataType) {
    const card = document.querySelector(`[data-type="${dataType}"]`);
    const content = card.querySelector('.quiver-card-content');
    const toggle = card.querySelector('.quiver-card-toggle i');
    
    if (content.classList.contains('hidden')) {
        // Expand the card
        content.classList.remove('hidden');
        toggle.style.transform = 'rotate(180deg)';
        
        // Load data for this card
        loadCardData(dataType, content);
        
        // Collapse other cards
        document.querySelectorAll('.quiver-card-content').forEach(otherContent => {
            if (otherContent !== content) {
                otherContent.classList.add('hidden');
                const otherToggle = otherContent.parentElement.querySelector('.quiver-card-toggle i');
                if (otherToggle) {
                    otherToggle.style.transform = 'rotate(0deg)';
                }
            }
        });
    } else {
        // Collapse the card
        content.classList.add('hidden');
        toggle.style.transform = 'rotate(0deg)';
    }
}

// Load card data from API
async function loadCardData(dataType, contentContainer) {
    currentDataView = dataType;
    
    try {
        // Show loading state
        contentContainer.innerHTML = '<div class="text-center py-8"><div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600 mx-auto"></div><p class="mt-2 text-gray-600">Loading data...</p></div>';
        
        let apiEndpoint;
        let response;
        
        switch (dataType) {
            case 'congress-trading':
                apiEndpoint = `${API_BASE_URL}/quiver/congressional-trading`;
                break;
            case 'government-contracts':
                apiEndpoint = `${API_BASE_URL}/quiver/government-contracts`;
                break;
            case 'house-trading':
                apiEndpoint = `${API_BASE_URL}/quiver/house-trading`;
                break;
            case 'senator-trading':
                apiEndpoint = `${API_BASE_URL}/quiver/senator-trading`;
                break;
            case 'lobbying':
                apiEndpoint = `${API_BASE_URL}/quiver/lobbying-activity`;
                break;
            case 'patent-momentum':
                apiEndpoint = `${API_BASE_URL}/quiver/patent-momentum`;
                break;
            default:
                throw new Error(`Unknown data type: ${dataType}`);
        }
        
        response = await fetch(apiEndpoint);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        
        // Set current data and filtered data
        currentData = data;
        filteredData = data;
        
        // Render the data table
        renderCardDataTable(dataType);
        
        // Show the modal
        showDataViewModal();
        
    } catch (error) {
        console.error('Error loading data:', error);
        contentContainer.innerHTML = `
            <div class="text-center py-8">
                <div class="text-red-600 mb-4">
                    <i class="bi bi-exclamation-triangle text-4xl"></i>
                </div>
                <p class="text-gray-600 mb-2">Failed to load data</p>
                <p class="text-sm text-gray-500">${error.message}</p>
                <button onclick="loadCardData('${dataType}', contentContainer)" class="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
                    Retry
                </button>
            </div>
        `;
    }
}

// Show data view modal
function showDataViewModal() {
    const modal = document.getElementById('data-view-modal');
    modal.classList.remove('hidden');
    
    // Update modal title based on current data view
    const modalTitle = document.getElementById('modal-title');
    const modalSubtitle = document.getElementById('modal-subtitle');
    
    switch (currentDataView) {
        case 'congress-trading':
            modalTitle.textContent = 'Historical Congress Trading';
            modalSubtitle.textContent = 'Comprehensive trading history by members of Congress';
            break;
        case 'government-contracts':
            modalTitle.textContent = 'Government Contracts';
            modalSubtitle.textContent = 'Government contract awards and spending data';
            break;
        case 'house-trading':
            modalTitle.textContent = 'House Trading';
            modalSubtitle.textContent = 'Trading activity by House of Representatives members';
            break;
        case 'senator-trading':
            modalTitle.textContent = 'Senator Trading';
            modalSubtitle.textContent = 'Trading activity by Senate members';
            break;
        case 'lobbying':
            modalTitle.textContent = 'Lobbying Activity';
            modalSubtitle.textContent = 'Lobbying expenditures and activities by companies';
            break;
        case 'patent-momentum':
            modalTitle.textContent = 'Patent Momentum';
            modalSubtitle.textContent = 'Patent activity and innovation metrics by company';
            break;
    }
    
    // Show/hide following section
    const followingSection = document.getElementById('following-section');
    if (followedCongressPeople.size > 0) {
        followingSection.classList.remove('hidden');
        updateFollowingSection();
    } else {
        followingSection.classList.add('hidden');
    }
}

// Close data view modal
function closeDataView() {
    const modal = document.getElementById('data-view-modal');
    modal.classList.add('hidden');
}

// Render data table
function renderCardDataTable(dataType) {
    const container = document.getElementById('data-table-container');
    
    if (!filteredData || filteredData.length === 0) {
        container.innerHTML = '<div class="text-center py-8 text-gray-500">No data available</div>';
        return;
    }
    
    // Determine columns based on data type
    let columns = [];
    switch (dataType) {
        case 'congress-trading':
            columns = ['Date', 'Ticker', 'Company', 'Congressperson', 'Party', 'Chamber', 'Trade Type', 'Amount', 'Owner'];
            break;
        case 'government-contracts':
            columns = ['Date', 'Ticker', 'Company', 'Contract Value', 'Agency', 'Contract Type', 'Description'];
            break;
        case 'house-trading':
            columns = ['Date', 'Ticker', 'Company', 'Representative', 'Party', 'State', 'Trade Type', 'Amount'];
            break;
        case 'senator-trading':
            columns = ['Date', 'Ticker', 'Company', 'Senator', 'Party', 'State', 'Trade Type', 'Amount'];
            break;
        case 'lobbying':
            columns = ['Date', 'Ticker', 'Company', 'Lobbying Firm', 'Amount', 'Issues', 'Registrant'];
            break;
        case 'patent-momentum':
            columns = ['Date', 'Ticker', 'Company', 'Patent Count', 'Patent Type', 'Innovation Score', 'Industry'];
            break;
    }
    
    // Create table HTML
    let tableHTML = `
        <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
                <tr>
                    ${columns.map(col => `<th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${col}</th>`).join('')}
                    <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
    `;
    
    // Add rows
    filteredData.forEach((item, index) => {
        tableHTML += '<tr class="hover:bg-gray-50">';
        
        // Add data cells based on data type
        switch (dataType) {
            case 'congress-trading':
                tableHTML += `
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.Date || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${item.Ticker || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.Company || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                        <button onclick="showCongressPersonPortfolio('${item.Congressperson || ''}')" class="text-blue-600 hover:text-blue-900 font-medium">
                            ${item.Congressperson || ''}
                        </button>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.Party || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.Chamber || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.TradeType || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">$${(item.Amount || 0).toLocaleString()}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.Owner || ''}</td>
                `;
                break;
            case 'government-contracts':
                tableHTML += `
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.Date || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${item.Ticker || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.Company || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">$${(item.ContractValue || 0).toLocaleString()}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.Agency || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.ContractType || ''}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.Description || ''}</td>
                `;
                break;
            // Add other cases as needed
            default:
                // Generic fallback
                columns.forEach(col => {
                    const key = col.toLowerCase().replace(/\s+/g, '');
                    tableHTML += `<td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item[key] || ''}</td>`;
                });
        }
        
        // Add action buttons
        tableHTML += `
            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                <button onclick="toggleFollow('${item.Congressperson || item.Company || ''}')" class="text-indigo-600 hover:text-indigo-900 mr-3">
                    <i class="bi ${followedCongressPeople.has(item.Congressperson || item.Company || '') ? 'bi-star-fill' : 'bi-star'}"></i>
                </button>
            </td>
        `;
        
        tableHTML += '</tr>';
    });
    
    tableHTML += `
            </tbody>
        </table>
    `;
    
    container.innerHTML = tableHTML;
}

// Show congressperson portfolio
async function showCongressPersonPortfolio(congressPersonName) {
    try {
        // Show loading state
        document.getElementById('modal-title').textContent = `${congressPersonName} - Loading...`;
        document.getElementById('modal-subtitle').textContent = 'Fetching portfolio data...';
        
        // Fetch portfolio data from backend API
        const response = await fetch(`${API_BASE_URL}/quiver/congressperson/${encodeURIComponent(congressPersonName)}/portfolio`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const portfolio = await response.json();
        
        // Update modal title and subtitle
        document.getElementById('modal-title').textContent = `${congressPersonName} - Portfolio Summary`;
        document.getElementById('modal-subtitle').textContent = `${portfolio.Chamber} • ${portfolio.State} • ${portfolio.Party === 'D' ? 'Democrat' : portfolio.Party === 'R' ? 'Republican' : 'Independent'}`;
        
        // Add back button
        const backButton = document.createElement('button');
        backButton.innerHTML = '<i class="bi bi-arrow-left mr-2"></i>Back to Data';
        backButton.className = 'mb-4 px-4 py-2 bg-gray-600 text-white rounded hover:bg-gray-700';
        backButton.onclick = () => renderCardDataTable(currentDataView);
        
        const container = document.getElementById('data-table-container');
        container.innerHTML = '';
        container.appendChild(backButton);
        
        // Render portfolio summary
        renderPortfolioSummary(portfolio);
        
    } catch (error) {
        console.error('Error fetching congressperson portfolio:', error);
        document.getElementById('modal-title').textContent = `${congressPersonName} - Error`;
        document.getElementById('modal-subtitle').textContent = 'Failed to load portfolio data';
        
        // Show error message
        const container = document.getElementById('data-table-container');
        container.innerHTML = `
            <div class="text-center py-8">
                <div class="text-red-600 mb-4">
                    <i class="bi bi-exclamation-triangle text-4xl"></i>
                </div>
                <p class="text-gray-600 mb-2">Failed to load portfolio data</p>
                <p class="text-sm text-gray-500">${error.message}</p>
                <button onclick="showCongressPersonPortfolio('${congressPersonName}')" class="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
                    Retry
                </button>
            </div>
        `;
    }
}

// Render portfolio summary
function renderPortfolioSummary(portfolio) {
    const container = document.getElementById('data-table-container');
    
    const summaryHTML = `
        <div class="bg-white rounded-lg shadow p-6">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div>
                    <h4 class="text-lg font-semibold text-gray-900 mb-4">Portfolio Overview</h4>
                    <div class="space-y-3">
                        <div class="flex justify-between">
                            <span class="text-gray-600">Total Portfolio Value:</span>
                            <span class="font-semibold">$${portfolio.Total_Portfolio_Value?.toLocaleString() || 'N/A'}</span>
                        </div>
                        <div class="flex justify-between">
                            <span class="text-gray-600">YTD Return:</span>
                            <span class="font-semibold ${portfolio.YTD_Return >= 0 ? 'text-green-600' : 'text-red-600'}">
                                ${portfolio.YTD_Return >= 0 ? '+' : ''}${portfolio.YTD_Return?.toFixed(2) || 'N/A'}%
                            </span>
                        </div>
                        <div class="flex justify-between">
                            <span class="text-gray-600">YTD Return Amount:</span>
                            <span class="font-semibold ${portfolio.YTD_Return_Amount >= 0 ? 'text-green-600' : 'text-red-600'}">
                                ${portfolio.YTD_Return_Amount >= 0 ? '+' : ''}$${portfolio.YTD_Return_Amount?.toLocaleString() || 'N/A'}
                            </span>
                        </div>
                    </div>
                </div>
                
                <div>
                    <h4 class="text-lg font-semibold text-gray-900 mb-4">Top Holdings</h4>
                    <div class="space-y-2">
                        ${portfolio.Top_Holdings?.map(holding => `
                            <div class="flex justify-between items-center p-2 bg-gray-50 rounded">
                                <span class="font-medium">${holding.Ticker}</span>
                                <span class="text-sm text-gray-600">$${holding.Value?.toLocaleString() || 'N/A'}</span>
                            </div>
                        `).join('') || 'No holdings data available'}
                    </div>
                </div>
            </div>
            
            <div class="mt-6">
                <h4 class="text-lg font-semibold text-gray-900 mb-4">Recent Trades</h4>
                <div class="space-y-2">
                    ${portfolio.Recent_Trades?.map(trade => `
                        <div class="flex justify-between items-center p-2 bg-gray-50 rounded">
                            <span class="font-medium">${trade.Ticker}</span>
                            <span class="text-sm text-gray-600">${trade.Type} ${trade.Shares} shares</span>
                            <span class="text-sm text-gray-600">$${trade.Amount?.toLocaleString() || 'N/A'}</span>
                        </div>
                    `).join('') || 'No recent trades data available'}
                </div>
            </div>
        </div>
    `;
    
    container.innerHTML += summaryHTML;
}

// Toggle follow status
function toggleFollow(name) {
    if (followedCongressPeople.has(name)) {
        followedCongressPeople.delete(name);
    } else {
        followedCongressPeople.add(name);
    }
    
    // Save to localStorage
    localStorage.setItem('followedCongressPeople', JSON.stringify(Array.from(followedCongressPeople)));
    
    // Update the UI
    updateFollowingSection();
    
    // Re-render the table to update follow buttons
    if (currentDataView) {
        renderCardDataTable(currentDataView);
    }
}

// Update following section
function updateFollowingSection() {
    const followingCount = document.getElementById('following-count');
    const followingList = document.getElementById('following-list');
    
    followingCount.textContent = followedCongressPeople.size;
    
    if (followedCongressPeople.size > 0) {
        followingList.innerHTML = Array.from(followedCongressPeople).map(name => `
            <span class="inline-flex items-center px-3 py-1 rounded-full text-sm bg-yellow-100 text-yellow-800 border border-yellow-200">
                <i class="bi bi-star-fill text-yellow-500 mr-1"></i>
                ${name}
                <button onclick="toggleFollow('${name}')" class="ml-2 text-yellow-600 hover:text-yellow-800">
                    <i class="bi bi-x"></i>
                </button>
            </span>
        `).join('');
    }
}

// Filter data
function filterData() {
    const filterValue = document.getElementById('filter-input').value.toLowerCase();
    
    if (!currentData) return;
    
    filteredData = currentData.filter(item => {
        // Search in various fields based on data type
        const searchableFields = [
            item.Ticker,
            item.Company,
            item.Congressperson,
            item.Representative,
            item.Senator,
            item.Party,
            item.State
        ].filter(Boolean);
        
        return searchableFields.some(field => 
            field.toString().toLowerCase().includes(filterValue)
        );
    });
    
    renderCardDataTable(currentDataView);
}

// Sort data
function sortData() {
    const sortValue = document.getElementById('sort-select').value;
    
    if (!filteredData) return;
    
    filteredData.sort((a, b) => {
        switch (sortValue) {
            case 'date-desc':
                return new Date(b.Date) - new Date(a.Date);
            case 'date-asc':
                return new Date(a.Date) - new Date(b.Date);
            case 'value-desc':
                return (b.Amount || b.ContractValue || 0) - (a.Amount || a.ContractValue || 0);
            case 'value-asc':
                return (a.Amount || a.ContractValue || 0) - (b.Amount || b.ContractValue || 0);
            case 'ticker':
                return (a.Ticker || '').localeCompare(b.Ticker || '');
            default:
                return 0;
        }
    });
    
    renderCardDataTable(currentDataView);
}

// Change data limit
function changeLimit() {
    const limitValue = parseInt(document.getElementById('limit-select').value);
    
    if (!currentData) return;
    
    filteredData = currentData.slice(0, limitValue);
    renderCardDataTable(currentDataView);
}

// Close modal when clicking outside
document.addEventListener('click', function(event) {
    const modal = document.getElementById('data-view-modal');
    if (event.target === modal) {
        closeDataView();
    }
});

// Close modal with Escape key
document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
        closeDataView();
    }
});
