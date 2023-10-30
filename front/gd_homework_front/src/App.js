import React, { useState, useEffect } from 'react';
import Category from './Category';
import SubcategoriesList from './Subcategories';
import {ReactComponent as Logo} from './images/logo.svg'
import * as settings from "./settings.js"

function App() {
  const [data, setData] = useState([]);
  const [selectedSubcategoryData, setSelectedSubcategoryData] = useState(null);
  const [svgWidth, setSvgWidth] = useState(0);
  const [svgHeight, setSvgHeight] = useState(0);
  const apiUrl = settings.API_SERVER;

  useEffect(() => {
    fetch(apiUrl + "getCategoriesAndSubcategories")
      .then((response) => response.json())
      .then((data) => setData(data))
      .catch((error) => {
        console.error(error);
      });
  }, []);

  useEffect(() => {
    const handleResize = () => {
      const container = document.getElementById('svg-container');
      if (container) {
        const rect = container.getBoundingClientRect();
        setSvgWidth(rect.width);
        setSvgHeight(rect.height);
      }
    };

    handleResize();

    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
    }, []);

    setCategoryNameOnButtonClick();

  return (
    <>
    <div className="App">
      <div className="container">
          <div id="svg-container" className="svg-container">
            <svg>
              <Logo width={svgWidth} height={svgHeight}/>
            </svg>
          </div>
      </div>
      <div className="data-container">
        <div className="dropdown-container">
          {data.map((category, index) => (
            <Category key={index} {...category} selectedSubcategoryData={selectedSubcategoryData} setSelectedSubcategoryData={setSelectedSubcategoryData}/>
          ))}
        </div>
       
        <div className="table-container">
        <div> 
          <p id='categoryClicked' className='category-name hidden'></p>
          <p id="currentCategory" className="category-name"></p>
          <p id='subcategoryClicked'></p>
        </div>
          {selectedSubcategoryData && (
            <SubcategoriesList selectedSubcategoryData={selectedSubcategoryData} />
          )}
        </div>
    </div>
    </div>
    </>
  );
}

function setCategoryNameOnButtonClick() {
  const buttons = document.querySelectorAll('.dropdown-button');
  const categoryClickedName = document.getElementById("categoryClicked");
  buttons.forEach((button) => {
    button.addEventListener('click', (event) => {
      const clickedButton = event.target;
      const buttonName = clickedButton.textContent;
      categoryClickedName.textContent = buttonName;
    });
  });
};
export default App;
