import {linkImg} from '@/utils/common'
const Blog = () => {

    return (
        <>
        <section className="home-slider js-fullheight owl-carousel">
      <div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('bg_1.jpg')})` }}>
      	<div className="overlay"></div>
        <div className="container">
          <div className="row slider-text justify-content-center align-items-center">

            <div className="col-md-7 col-sm-12 text-center ftco-animate">
            	<h1 className="mb-3 mt-5 bread">Blog</h1>
	            <p className="breadcrumbs"><span className="mr-2"><a href="/Home">Home</a></span> <span>Blog</span></p>
            </div>

          </div>
        </div>
      </div>
    </section>

		<section className="ftco-section">
			<div className="container">
				<div className="row justify-content-center mb-5 pb-3">
          <div className="col-md-7 heading-section ftco-animate">
            <h2 className="mb-4">Recent Blog</h2>
            <p>Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in</p>
          </div>
        </div>	
				<div className="row">
          <div className="col-md-4 ftco-animate">
            <div className="blog-entry">
              <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_1.jpg')})` }}>
              </a>
              <div className="text d-flex py-4">
                <div className="meta mb-3">
                  <div><a href="#">Sep. 20, 2018</a></div>
                  <div><a href="#">Admin</a></div>
                  <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                </div>
                <div className="desc pl-3">
	                <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
	              </div>
              </div>
            </div>
          </div>
          <div className="col-md-4 ftco-animate">
            <div className="blog-entry" data-aos-delay="100">
              <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_2.jpg')})` }}>
              </a>
              <div className="text d-flex py-4">
                <div className="meta mb-3">
                  <div><a href="#">Sep. 20, 2018</a></div>
                  <div><a href="#">Admin</a></div>
                  <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                </div>
                <div className="desc pl-3">
	                <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
	              </div>
              </div>
            </div>
          </div>
          <div className="col-md-4 ftco-animate">
            <div className="blog-entry" data-aos-delay="200">
              <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_3.jpg')})` }}>
              </a>
              <div className="text d-flex py-4">
                <div className="meta mb-3">
                  <div><a href="#">Sep. 20, 2018</a></div>
                  <div><a href="#">Admin</a></div>
                  <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                </div>
                <div className="desc pl-3">
	                <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
	              </div>
              </div>
            </div>
          </div>
          <div className="col-md-4 ftco-animate">
            <div className="blog-entry">
              <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_4.jpg')})` }}>
              </a>
              <div className="text d-flex py-4">
                <div className="meta mb-3">
                  <div><a href="#">Sep. 20, 2018</a></div>
                  <div><a href="#">Admin</a></div>
                  <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                </div>
                <div className="desc pl-3">
	                <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
	              </div>
              </div>
            </div>
          </div>
          <div className="col-md-4 ftco-animate">
            <div className="blog-entry" data-aos-delay="100">
              <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_5.jpg')})` }}>
              </a>
              <div className="text d-flex py-4">
                <div className="meta mb-3">
                  <div><a href="#">Sep. 20, 2018</a></div>
                  <div><a href="#">Admin</a></div>
                  <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                </div>
                <div className="desc pl-3">
	                <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
	              </div>
              </div>
            </div>
          </div>
          <div className="col-md-4 ftco-animate">
            <div className="blog-entry" data-aos-delay="200">
              <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_6.jpg')})` }}>
              </a>
              <div className="text d-flex py-4">
                <div className="meta mb-3">
                  <div><a href="#">Sep. 20, 2018</a></div>
                  <div><a href="#">Admin</a></div>
                  <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                </div>
                <div className="desc pl-3">
	                <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
	              </div>
              </div>
            </div>
          </div>
        </div>
			</div>
		</section>
        </>
    )
}

export default Blog;